using UnityEngine;

//C#中 泛型知识点
//设计模式 单例模式的知识点
//继承了 MonoBehaviour 的 单例模式对象 需要我们自己保证它的位移性
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    protected virtual void Awake()
    {
        instance = this as T;
    }

    public static T GetInstance()
    {

        return instance;
    }

#if UNITY_EDITOR
    //private void OnEnable()
    //{
    //    instance = this as T;
    //}
#endif

public static T Instance
{
    get
    {
        if (instance == null)
        {
            instance = Object.FindFirstObjectByType<T>();
            if (instance == null)
            {
                Debug.LogError("无法在场景中找到:" + typeof(T));
                return null;
            }
        }
        return instance;
    }
}

}
