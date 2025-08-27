using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 计时管理器，高效处理大量计时器
/// 核心优势：每帧仅检查最快到期的计时器，大幅降低性能消耗
/// </summary>
public class TimerManager : MonoBehaviour
{
    // 单例实例
    public static TimerManager Instance { get; private set; }

    // 计时器数据结构
    private struct Timer
    {
        public string id;               // 计时器唯一标识
        public float expireTime;        // 到期时间
        public float interval;          // 间隔时间（循环计时用）
        public Action callback;         // 回调函数
        public bool isLooping;          // 是否循环
        public bool isPaused;           // 是否暂停
        public float pauseTime;         // 暂停时的时间点
    }

    // 【替换】自定义优先队列：按 expireTime 升序排序（最快到期排前面）
    private class SimplePriorityQueue
    {
        private List<(Timer timer, float priority)> _queue = new List<(Timer, float)>();

        /// <summary>
        /// 入队（自动按 priority 升序维护队列）
        /// </summary>
        public void Enqueue(Timer timer, float priority)
        {
            _queue.Add((timer, priority));
            // 按 priority 升序排序，保证最快到期的在队首
            _queue = _queue.OrderBy(item => item.priority).ToList();
        }

        /// <summary>
        /// 出队（取队首元素并移除）
        /// </summary>
        public (Timer, float) Dequeue()
        {
            if (_queue.Count == 0)
                throw new InvalidOperationException("优先队列为空，无法出队");

            var first = _queue[0];
            _queue.RemoveAt(0);
            return first;
        }

        /// <summary>
        /// 查看队首元素（不移除）
        /// </summary>
        public (Timer, float) Peek()
        {
            if (_queue.Count == 0)
                throw new InvalidOperationException("优先队列为空，无法查看");

            return _queue[0];
        }

        /// <summary>
        /// 当前队列元素数量
        /// </summary>
        public int Count => _queue.Count;
    }

    // 替换为自定义优先队列
    private SimplePriorityQueue _timerQueue = new SimplePriorityQueue();

    // 用于快速查找和移除计时器的字典
    private Dictionary<string, Timer> _activeTimers = new Dictionary<string, Timer>();

    // 待移除的计时器ID（避免在遍历中修改集合）
    private HashSet<string> _pendingRemovals = new HashSet<string>();

    private bool _isPaused = false;
    private float _pauseStartTime;

    private void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_isPaused) return;

        // 处理所有已到期的计时器
        while (_timerQueue.Count > 0)
        {
            // 查看最早到期的计时器
            var peeked = _timerQueue.Peek();
            var currentTimer = peeked.Item1; // 注意：自定义队列返回 (Timer, float)，用 Item1 取 Timer

            // 检查是否已被标记为移除
            if (_pendingRemovals.Contains(currentTimer.id))
            {
                _timerQueue.Dequeue();
                _activeTimers.Remove(currentTimer.id);
                _pendingRemovals.Remove(currentTimer.id);
                continue;
            }

            // 检查是否到期
            if (Time.time >= currentTimer.expireTime)
            {
                // 出队并执行回调
                var completedTimer = _timerQueue.Dequeue().Item1;
                _activeTimers.Remove(completedTimer.id);

                try
                {
                    completedTimer.callback?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"计时器回调执行错误: {e.Message}");
                }

                // 如果是循环计时器，重新加入队列
                if (completedTimer.isLooping)
                {
                    completedTimer.expireTime = Time.time + completedTimer.interval;
                    AddTimerInternal(completedTimer);
                }
            }
            else
            {
                // 未到期，退出循环（后面的计时器更晚到期）
                break;
            }
        }
    }

    /// <summary>
    /// 添加一次性计时器
    /// </summary>
    /// <param name="delay">延迟时间（秒）</param>
    /// <param name="callback">到期回调</param>
    /// <returns>计时器ID，用于移除</returns>
    public string AddTimer(float delay, Action callback)
    {
        return AddTimer(delay, callback, false, 0);
    }

    /// <summary>
    /// 添加循环计时器
    /// </summary>
    /// <param name="interval">循环间隔（秒）</param>
    /// <param name="callback">每次到期回调</param>
    /// <returns>计时器ID，用于移除</returns>
    public string AddLoopTimer(float interval, Action callback)
    {
        return AddTimer(interval, callback, true, interval);
    }

    /// <summary>
    /// 内部计时器添加实现
    /// </summary>
    private string AddTimer(float delay, Action callback, bool isLooping, float interval)
    {
        if (delay < 0)
        {
            Debug.LogWarning("计时器延迟时间不能为负数");
            delay = 0;
        }

        var timer = new Timer
        {
            id = Guid.NewGuid().ToString(),
            expireTime = Time.time + delay,
            interval = interval,
            callback = callback,
            isLooping = isLooping,
            isPaused = false
        };

        AddTimerInternal(timer);
        return timer.id;
    }

    private void AddTimerInternal(Timer timer)
    {
        // 自定义队列用 expireTime 作为排序依据（升序）
        _timerQueue.Enqueue(timer, timer.expireTime);
        _activeTimers[timer.id] = timer;
    }

    /// <summary>
    /// 移除计时器
    /// </summary>
    /// <param name="timerId">要移除的计时器ID</param>
    /// <returns>是否移除成功</returns>
    public bool RemoveTimer(string timerId)
    {
        if (string.IsNullOrEmpty(timerId)) return false;

        if (_activeTimers.ContainsKey(timerId))
        {
            _pendingRemovals.Add(timerId);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 暂停指定计时器
    /// </summary>
    public bool PauseTimer(string timerId)
    {
        if (_activeTimers.TryGetValue(timerId, out var timer) && !timer.isPaused)
        {
            timer.isPaused = true;
            timer.pauseTime = Time.time;
            _activeTimers[timerId] = timer;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 恢复指定计时器
    /// </summary>
    public bool ResumeTimer(string timerId)
    {
        if (_activeTimers.TryGetValue(timerId, out var timer) && timer.isPaused)
        {
            // 计算暂停的时间，调整到期时间
            float pauseDuration = Time.time - timer.pauseTime;
            timer.expireTime += pauseDuration;
            timer.isPaused = false;
            _activeTimers[timerId] = timer;

            // 重新加入队列（因为到期时间已更新）
            _pendingRemovals.Add(timerId);
            AddTimerInternal(timer);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 暂停所有计时器
    /// </summary>
    public void PauseAll()
    {
        if (_isPaused) return;

        _isPaused = true;
        _pauseStartTime = Time.time;
    }

    /// <summary>
    /// 恢复所有计时器
    /// </summary>
    public void ResumeAll()
    {
        if (!_isPaused) return;

        float pauseDuration = Time.time - _pauseStartTime;
        _isPaused = false;

        // 调整所有计时器的到期时间
        var tempTimers = new List<Timer>(_activeTimers.Values);
        _activeTimers.Clear();
        _timerQueue = new SimplePriorityQueue(); // 重置自定义队列
        _pendingRemovals.Clear();

        foreach (var timer in tempTimers)
        {
            var adjustedTimer = timer;
            adjustedTimer.expireTime += pauseDuration;
            AddTimerInternal(adjustedTimer);
        }
    }

    /// <summary>
    /// 清除所有计时器
    /// </summary>
    public void ClearAllTimers()
    {
        _timerQueue = new SimplePriorityQueue();
        _activeTimers.Clear();
        _pendingRemovals.Clear();
    }

    /// <summary>
    /// 获取当前活跃计时器数量
    /// </summary>
    public int GetActiveTimerCount()
    {
        return _activeTimers.Count - _pendingRemovals.Count;
    }
}
