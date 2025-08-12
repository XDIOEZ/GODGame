using System;
using UnityEngine;
using MemoryPack;

public static class PersistentDataUtil_MPack
{
    /// <summary>
    /// �������ݵ� PlayerPrefs��ʹ�� MemoryPack ���л����洢Ϊ Base64 �ַ���
    /// </summary>
    public static void Save<T>(string key, T data)
    {
        try
        {
            // ���л�Ϊ������
            byte[] bytes = MemoryPackSerializer.Serialize(data);
            // תΪ Base64 �洢
            string base64 = Convert.ToBase64String(bytes);
            PlayerPrefs.SetString(key, base64);
            PlayerPrefs.Save();
            Debug.Log($"�����ѱ��棬Key={key}");
        }
        catch (Exception e)
        {
            Debug.LogError($"��������ʧ�ܣ�Key={key} Exception: {e}");
        }
    }

    /// <summary>
    /// �� PlayerPrefs ��ȡ���ݣ������л�Ϊ T ����
    /// �����ȡ����������Ĭ��ֵ defaultValue
    /// </summary>
    public static T Load<T>(string key, T defaultValue = default)
    {
        try
        {
            if (!PlayerPrefs.HasKey(key))
            {
                Debug.LogWarning($"δ�ҵ�Key={key}�����ݣ�����Ĭ��ֵ");
                return defaultValue;
            }

            string base64 = PlayerPrefs.GetString(key);
            byte[] bytes = Convert.FromBase64String(base64);
            T data = MemoryPackSerializer.Deserialize<T>(bytes);
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"��������ʧ�ܣ�Key={key} Exception: {e}");
            return defaultValue;
        }
    }

    /// <summary>
    /// ɾ��ָ�� Key ������
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
