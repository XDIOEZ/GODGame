using System;
using UnityEngine;
using Sirenix.Serialization;

namespace ProjectBase.PersistentData
{
    public static class PersistentDataUtil_Odin
    {
        public static void Save<T>(string key, T data)
        {
            try
            {
                // 老版本用 byte[] 输出
                byte[] bytes = SerializationUtility.SerializeValue<T>(data, DataFormat.JSON);
                string json = System.Text.Encoding.UTF8.GetString(bytes);
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
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
                defaultValue = SerializationUtility.DeserializeValue<T>(bytes, DataFormat.JSON);
                return defaultValue;
            }
            catch (Exception e)
            {
                Debug.LogError($"加载失败，Key={key} 异常: {e}");
                return defaultValue;
            }
        }

        public static void Delete(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
                Debug.Log($"已删除 Key={key} 的数据");
            }
        }

        public static void ClearAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("已清空所有 PlayerPrefs 数据");
        }

        [Serializable]
        public class PlayerData
        {
            public string name = "Player";
            public int coin = 100;
            public Vector2 position = new Vector2(10, 10);

            public override string ToString()
            {
                return $"name={name}, coin={coin}, position={position}";
            }
        }
    }
}
