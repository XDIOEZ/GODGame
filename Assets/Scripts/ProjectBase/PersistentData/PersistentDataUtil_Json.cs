using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Plastic.Newtonsoft.Json;

public static class PersistentDataUtil_Json
{
    /// <summary>
    /// 保存任意类型数据到 PlayerPrefs（Json序列化）
    /// </summary>
    public static void Save<T>(string key, T data)
    {
        try
        {
            string json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            Debug.Log($"保存成功，Key={key}");
        }
        catch (Exception e)
        {
            Debug.LogError($"保存失败，Key={key} 异常: {e}");
        }
    }

    public class PlayerData
    {
        public string name = "Player";
        public int coin = 100;

        public override string ToString()
        {
            return $"name={name},coin={coin}";
        }
    }


    /// <summary>
    /// 读取任意类型数据，读取失败返回 defaultValue
    /// </summary>
    public static T Load<T>(string key, ref T defaultValue)
    {
        try
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"未找到 Key={key} 的数据，返回默认值");
                return defaultValue;
            }

            string json = PlayerPrefs.GetString(key);
            defaultValue = JsonConvert.DeserializeObject<T>(json);
            return defaultValue;
        }
        catch (Exception e)
        {
            Debug.LogError($"加载失败，Key={key} 异常: {e}");
            return defaultValue;
        }
    }

    /// <summary>
    /// 删除指定 Key 数据
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
