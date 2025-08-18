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
                // �ϰ汾�� byte[] ���
                byte[] bytes = SerializationUtility.SerializeValue<T>(data, DataFormat.JSON);
                string json = System.Text.Encoding.UTF8.GetString(bytes);
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
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
                defaultValue = SerializationUtility.DeserializeValue<T>(bytes, DataFormat.JSON);
                return defaultValue;
            }
            catch (Exception e)
            {
                Debug.LogError($"����ʧ�ܣ�Key={key} �쳣: {e}");
                return defaultValue;
            }
        }

        public static void Delete(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
                Debug.Log($"��ɾ�� Key={key} ������");
            }
        }

        public static void ClearAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("��������� PlayerPrefs ����");
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
