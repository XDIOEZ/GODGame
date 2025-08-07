using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 泛型自动创建的单例基类
/// 继承这个类后，子类可以全局访问，自动跨场景，且不会重复创建
/// </summary>
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    /// <summary>
    /// 单例全局访问
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 优先寻找场景里是否已经存在
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    // 场景不存在就自动创建
                    GameObject obj = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(obj);
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    /// <summary>
    /// Awake 防呆，防止场景中手动拖拽多个对象导致冲突
    /// </summary>
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            // 场景里如果出现重复对象，自动销毁
            Destroy(gameObject);
        }
    }
}
