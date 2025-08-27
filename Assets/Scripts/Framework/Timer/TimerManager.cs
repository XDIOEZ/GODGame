using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// ��ʱ����������Ч���������ʱ��
/// �������ƣ�ÿ֡�������쵽�ڵļ�ʱ�������������������
/// </summary>
public class TimerManager : MonoBehaviour
{
    // ����ʵ��
    public static TimerManager Instance { get; private set; }

    // ��ʱ�����ݽṹ
    private struct Timer
    {
        public string id;               // ��ʱ��Ψһ��ʶ
        public float expireTime;        // ����ʱ��
        public float interval;          // ���ʱ�䣨ѭ����ʱ�ã�
        public Action callback;         // �ص�����
        public bool isLooping;          // �Ƿ�ѭ��
        public bool isPaused;           // �Ƿ���ͣ
        public float pauseTime;         // ��ͣʱ��ʱ���
    }

    // ���滻���Զ������ȶ��У��� expireTime ����������쵽����ǰ�棩
    private class SimplePriorityQueue
    {
        private List<(Timer timer, float priority)> _queue = new List<(Timer, float)>();

        /// <summary>
        /// ��ӣ��Զ��� priority ����ά�����У�
        /// </summary>
        public void Enqueue(Timer timer, float priority)
        {
            _queue.Add((timer, priority));
            // �� priority �������򣬱�֤��쵽�ڵ��ڶ���
            _queue = _queue.OrderBy(item => item.priority).ToList();
        }

        /// <summary>
        /// ���ӣ�ȡ����Ԫ�ز��Ƴ���
        /// </summary>
        public (Timer, float) Dequeue()
        {
            if (_queue.Count == 0)
                throw new InvalidOperationException("���ȶ���Ϊ�գ��޷�����");

            var first = _queue[0];
            _queue.RemoveAt(0);
            return first;
        }

        /// <summary>
        /// �鿴����Ԫ�أ����Ƴ���
        /// </summary>
        public (Timer, float) Peek()
        {
            if (_queue.Count == 0)
                throw new InvalidOperationException("���ȶ���Ϊ�գ��޷��鿴");

            return _queue[0];
        }

        /// <summary>
        /// ��ǰ����Ԫ������
        /// </summary>
        public int Count => _queue.Count;
    }

    // �滻Ϊ�Զ������ȶ���
    private SimplePriorityQueue _timerQueue = new SimplePriorityQueue();

    // ���ڿ��ٲ��Һ��Ƴ���ʱ�����ֵ�
    private Dictionary<string, Timer> _activeTimers = new Dictionary<string, Timer>();

    // ���Ƴ��ļ�ʱ��ID�������ڱ������޸ļ��ϣ�
    private HashSet<string> _pendingRemovals = new HashSet<string>();

    private bool _isPaused = false;
    private float _pauseStartTime;

    private void Awake()
    {
        // ������ʼ��
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

        // ���������ѵ��ڵļ�ʱ��
        while (_timerQueue.Count > 0)
        {
            // �鿴���絽�ڵļ�ʱ��
            var peeked = _timerQueue.Peek();
            var currentTimer = peeked.Item1; // ע�⣺�Զ�����з��� (Timer, float)���� Item1 ȡ Timer

            // ����Ƿ��ѱ����Ϊ�Ƴ�
            if (_pendingRemovals.Contains(currentTimer.id))
            {
                _timerQueue.Dequeue();
                _activeTimers.Remove(currentTimer.id);
                _pendingRemovals.Remove(currentTimer.id);
                continue;
            }

            // ����Ƿ���
            if (Time.time >= currentTimer.expireTime)
            {
                // ���Ӳ�ִ�лص�
                var completedTimer = _timerQueue.Dequeue().Item1;
                _activeTimers.Remove(completedTimer.id);

                try
                {
                    completedTimer.callback?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"��ʱ���ص�ִ�д���: {e.Message}");
                }

                // �����ѭ����ʱ�������¼������
                if (completedTimer.isLooping)
                {
                    completedTimer.expireTime = Time.time + completedTimer.interval;
                    AddTimerInternal(completedTimer);
                }
            }
            else
            {
                // δ���ڣ��˳�ѭ��������ļ�ʱ�������ڣ�
                break;
            }
        }
    }

    /// <summary>
    /// ���һ���Լ�ʱ��
    /// </summary>
    /// <param name="delay">�ӳ�ʱ�䣨�룩</param>
    /// <param name="callback">���ڻص�</param>
    /// <returns>��ʱ��ID�������Ƴ�</returns>
    public string AddTimer(float delay, Action callback)
    {
        return AddTimer(delay, callback, false, 0);
    }

    /// <summary>
    /// ���ѭ����ʱ��
    /// </summary>
    /// <param name="interval">ѭ��������룩</param>
    /// <param name="callback">ÿ�ε��ڻص�</param>
    /// <returns>��ʱ��ID�������Ƴ�</returns>
    public string AddLoopTimer(float interval, Action callback)
    {
        return AddTimer(interval, callback, true, interval);
    }

    /// <summary>
    /// �ڲ���ʱ�����ʵ��
    /// </summary>
    private string AddTimer(float delay, Action callback, bool isLooping, float interval)
    {
        if (delay < 0)
        {
            Debug.LogWarning("��ʱ���ӳ�ʱ�䲻��Ϊ����");
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
        // �Զ�������� expireTime ��Ϊ�������ݣ�����
        _timerQueue.Enqueue(timer, timer.expireTime);
        _activeTimers[timer.id] = timer;
    }

    /// <summary>
    /// �Ƴ���ʱ��
    /// </summary>
    /// <param name="timerId">Ҫ�Ƴ��ļ�ʱ��ID</param>
    /// <returns>�Ƿ��Ƴ��ɹ�</returns>
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
    /// ��ָͣ����ʱ��
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
    /// �ָ�ָ����ʱ��
    /// </summary>
    public bool ResumeTimer(string timerId)
    {
        if (_activeTimers.TryGetValue(timerId, out var timer) && timer.isPaused)
        {
            // ������ͣ��ʱ�䣬��������ʱ��
            float pauseDuration = Time.time - timer.pauseTime;
            timer.expireTime += pauseDuration;
            timer.isPaused = false;
            _activeTimers[timerId] = timer;

            // ���¼�����У���Ϊ����ʱ���Ѹ��£�
            _pendingRemovals.Add(timerId);
            AddTimerInternal(timer);
            return true;
        }
        return false;
    }

    /// <summary>
    /// ��ͣ���м�ʱ��
    /// </summary>
    public void PauseAll()
    {
        if (_isPaused) return;

        _isPaused = true;
        _pauseStartTime = Time.time;
    }

    /// <summary>
    /// �ָ����м�ʱ��
    /// </summary>
    public void ResumeAll()
    {
        if (!_isPaused) return;

        float pauseDuration = Time.time - _pauseStartTime;
        _isPaused = false;

        // �������м�ʱ���ĵ���ʱ��
        var tempTimers = new List<Timer>(_activeTimers.Values);
        _activeTimers.Clear();
        _timerQueue = new SimplePriorityQueue(); // �����Զ������
        _pendingRemovals.Clear();

        foreach (var timer in tempTimers)
        {
            var adjustedTimer = timer;
            adjustedTimer.expireTime += pauseDuration;
            AddTimerInternal(adjustedTimer);
        }
    }

    /// <summary>
    /// ������м�ʱ��
    /// </summary>
    public void ClearAllTimers()
    {
        _timerQueue = new SimplePriorityQueue();
        _activeTimers.Clear();
        _pendingRemovals.Clear();
    }

    /// <summary>
    /// ��ȡ��ǰ��Ծ��ʱ������
    /// </summary>
    public int GetActiveTimerCount()
    {
        return _activeTimers.Count - _pendingRemovals.Count;
    }
}
