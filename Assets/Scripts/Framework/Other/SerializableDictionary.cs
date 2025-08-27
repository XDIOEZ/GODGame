using System.Collections.Generic;
using UnityEngine;

// 1. ��ֵ���ౣ�����л�
[System.Serializable]
public class SerializableKeyValuePair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public SerializableKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}

// 2. �������������ֵ������ [System.Serializable] ��ǩ
[System.Serializable] // �ؼ�����Unityʶ�����л������
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    // 3. ȷ��List�ֶ���public��[SerializeField]�������㣩
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // �����л�ʱ�ؽ��ֵ�
    public void OnAfterDeserialize()
    {
        Clear();
        // ��������Խ��
        int count = Mathf.Min(keys.Count, values.Count);
        for (int i = 0; i < count; i++)
        {
            if (!ContainsKey(keys[i]))
                Add(keys[i], values[i]);
        }
    }

    // ���л�ʱͬ����List
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
}
