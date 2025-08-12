using System;
using UnityEngine;
using MemoryPack;

public static class PersistentDataUtil_MPack
{
    /// <summary>
    /// 保存数据到 PlayerPrefs，使用 MemoryPack 序列化，存储为 Base64 字符串
    /// </summary>
    public static void Save<T>(string key, T data)
    {
        try
        {
            // 序列化为二进制
            byte[] bytes = MemoryPackSerializer.Serialize(data);
            // 转为 Base64 存储
            string base64 = Convert.ToBase64String(bytes);
            PlayerPrefs.SetString(key, base64);
            PlayerPrefs.Save();
            Debug.Log($"数据已保存，Key={key}");
        }
        catch (Exception e)
        {
            Debug.LogError($"保存数据失败，Key={key} Exception: {e}");
        }
    }

    /// <summary>
    /// 从 PlayerPrefs 读取数据，反序列化为 T 类型
    /// 如果读取不到，返回默认值 defaultValue
    /// </summary>
    public static T Load<T>(string key, T defaultValue = default)
    {
        try
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"未找到Key={key}的数据，返回默认值");
                return defaultValue;
            }

            string base64 = PlayerPrefs.GetString(key);
            byte[] bytes = Convert.FromBase64String(base64);
            T data = MemoryPackSerializer.Deserialize<T>(bytes);
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"加载数据失败，Key={key} Exception: {e}");
            return defaultValue;
        }
    }

    /// <summary>
    /// 删除指定 Key 的数据
    /// </summary>
    public static void Delete(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            Debug.Log($"已删除 Key={key} 的数据");
        }
    }

    /// <summary>
    /// 清空所有 PlayerPrefs 数据（慎用）
    /// </summary>
    public static void ClearAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("已清空所有 PlayerPrefs 数据");
    }
}
