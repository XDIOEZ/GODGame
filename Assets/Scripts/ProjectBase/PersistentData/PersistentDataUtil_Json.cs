using System;
using UnityEngine;
using Newtonsoft.Json;

public static class PersistentDataUtil_Json
{
    /// <summary>
    /// ���������������ݵ� PlayerPrefs��Json���л���
    /// </summary>
    public static void Save<T>(string key, T data)
    {
        try
        {
            string json = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
            Debug.Log($"����ɹ���Key={key}");
        }
        catch (Exception e)
        {
            Debug.LogError($"����ʧ�ܣ�Key={key} �쳣: {e}");
        }
    }

    /// <summary>
    /// ��ȡ�����������ݣ���ȡʧ�ܷ��� defaultValue
    /// </summary>
    public static T Load<T>(string key, T defaultValue = default)
    {
        try
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"δ�ҵ� Key={key} �����ݣ�����Ĭ��ֵ");
                return defaultValue;
            }

            string json = PlayerPrefs.GetString(key);
            T data = JsonConvert.DeserializeObject<T>(json);
            return data;
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
