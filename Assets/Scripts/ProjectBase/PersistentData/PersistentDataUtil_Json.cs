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
            // ʹ�� converters �� Unity ���Ϳ����л�
            string json = JsonConvert.SerializeObject(data, new JsonConverter[] { new Vector2Converter() });
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            Debug.Log($"����ɹ���Key={key}");
        }
        catch (Exception e)
        {
            Debug.LogError($"����ʧ�ܣ�Key={key} �쳣: {e}");
        }
    }

    public static T Load<T>(string key, ref T defaultValue)
    {
        try
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"δ�ҵ� Key={key} �����ݣ�����Ĭ��ֵ");
                return defaultValue;
            }

            string json = PlayerPrefs.GetString(key);
            // ʹ����ͬ�� converter �����л�
            defaultValue = JsonConvert.DeserializeObject<T>(json, new JsonConverter[] { new Vector2Converter() });
            return defaultValue;
        }
        catch (Exception e)
        {
            Debug.LogError($"����ʧ�ܣ�Key={key} �쳣: {e}");
            return defaultValue;
        }
    }


    /// <summary>
    /// ɾ��ָ�� Key ����
    /// </summary>
    public static void Delete(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            Debug.Log($"��ɾ�� Key={key} ������");
        }
    }

    /// <summary>
    /// ������� PlayerPrefs ���ݣ����ã�
    /// </summary>
    public static void ClearAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("��������� PlayerPrefs ����");
    }
}