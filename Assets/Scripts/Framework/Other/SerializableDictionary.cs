using System.Collections.Generic;
using UnityEngine;

// 1. 键值对类保持序列化
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

// 2. 核心修正：给字典类添加 [System.Serializable] 标签
[System.Serializable] // 关键：让Unity识别并序列化这个类
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    // 3. 确保List字段是public或[SerializeField]（已满足）
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // 反序列化时重建字典
    public void OnAfterDeserialize()
    {
        Clear();
        // 避免索引越界
        int count = Mathf.Min(keys.Count, values.Count);
        for (int i = 0; i < count; i++)
        {
            if (!ContainsKey(keys[i]))
                Add(keys[i], values[i]);
        }
    }

    // 序列化时同步到List
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
