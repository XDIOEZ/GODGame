using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局事件管理器（单例模式）
/// 支持泛型事件类型，自动处理委托的注册与注销
/// </summary>
public class EventManager : MonoBehaviour
{
    // 单例实例
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 确保场景中存在一个挂载EventManager的GameObject
                GameObject obj = new GameObject("EventManager");
                _instance = obj.AddComponent<EventManager>();
                DontDestroyOnLoad(obj); // 跨场景保留
                Debug.Log("事件系统正常启动");
            }
            return _instance;
        }
    }

    // 事件字典：Key=事件类型，Value=该类型对应的所有回调委托
    private Dictionary<Type, Delegate> _eventDict = new Dictionary<Type, Delegate>();

    // 线程锁（确保多线程环境下的安全性）
    private readonly object _lock = new object();

    /// <summary>
    /// 注册事件监听
    /// </summary>
    /// <typeparam name="T">事件数据类型（继承自IEvent）</typeparam>
    /// <param name="onEvent">事件触发时的回调</param>
    public void On<T>(Action<T> onEvent) where T : IEvent
    {
        lock (_lock)
        {
            Type eventType = typeof(T);
            // 如果字典中没有该事件类型，添加一个空委托
            if (!_eventDict.ContainsKey(eventType))
            {
                _eventDict[eventType] = null;
            }
            // 合并委托（多播委托）
            _eventDict[eventType] = (Action<T>)_eventDict[eventType] + onEvent;
        }
    }

    /// <summary>
    /// 取消事件监听
    /// </summary>
    /// <typeparam name="T">事件数据类型</typeparam>
    /// <param name="onEvent">需要取消的回调（必须与注册时的委托一致）</param>
    public void Off<T>(Action<T> onEvent) where T : IEvent
    {
        lock (_lock)
        {
            Type eventType = typeof(T);
            if (_eventDict.TryGetValue(eventType, out Delegate existingDelegate))
            {
                // 移除指定委托
                existingDelegate = (Action<T>)existingDelegate - onEvent;
                // 如果委托为空，从字典中移除该事件类型
                if (existingDelegate == null)
                {
                    _eventDict.Remove(eventType);
                }
                else
                {
                    _eventDict[eventType] = existingDelegate;
                }
            }
        }
    }

    /// <summary>
    /// 发布事件（触发所有监听该事件的回调）
    /// </summary>
    /// <typeparam name="T">事件数据类型</typeparam>
    /// <param name="eventData">事件携带的数据</param>
    public void Emit<T>(T eventData) where T : IEvent
    {
        Delegate targetDelegate;
        lock (_lock)
        {
            // 尝试获取该事件类型的委托
            if (!_eventDict.TryGetValue(typeof(T), out targetDelegate))
            {
                return; // 没有监听者，直接返回
            }
        }

        // 执行所有注册的回调（复制一份委托防止执行中被修改）
        if (targetDelegate is Action<T> action)
        {
            try
            {
                action.Invoke(eventData);
            }
            catch (Exception e)
            {
                Debug.LogError($"事件{typeof(T).Name}触发失败：{e.Message}");
            }
        }
    }

    /// <summary>
    /// 清除所有事件监听（慎用，一般用于场景切换或全局重置）
    /// </summary>
    public void ClearAllEvents()
    {
        lock (_lock)
        {
            _eventDict.Clear();
        }
        Debug.Log("所有事件监听已清除");
    }
}
