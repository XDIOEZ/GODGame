using System;
using UnityEngine;
using Sirenix.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.UnityConverters.Math;
public static class PersistentDataUtil_Json
{

    public class PlayerData
    {
        public string name = "Player";
        public int coin = 100;
        public Vector2 position = new Vector2(10, 10);

        public override string ToString()
        {
            return $"name={name},coin={coin}";
        }
    }


    public static void Save<T>(string key, T data)
    {
        try
        {
            // 使用 converters 让 Unity 类型可序列化
            string json = JsonConvert.SerializeObject(data, new JsonConverter[] { new Vector2Converter() });
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            Debug.Log($"保存成功，Key={key}");
        }
        catch (Exception e)
        {
            Debug.LogError($"保存失败，Key={key} 异常: {e}");
        }
    }

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
            // 使用相同的 converter 反序列化
            defaultValue = JsonConvert.DeserializeObject<T>(json, new JsonConverter[] { new Vector2Converter() });
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