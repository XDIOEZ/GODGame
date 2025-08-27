using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Engine;


/// <summary>
/// 推进器基本结构
/// </summary>
[System.Serializable]
public struct ThrustData  // 结构体建议用Pascal命名法（首字母大写）
{
    // 字段设为public或添加[SerializeField]，确保能在Inspector中编辑
    [Header("ID")][SerializeField] private int thrustID;
    [Header("引擎名")][SerializeField] private string thrustName;
    [Header("引擎种类")][SerializeField] private EngineType engineType;
    [Header("引擎推力")][SerializeField] private int thrustPower;
    [Header("引擎旋转速度")][SerializeField] private int torqueForcePower;

    // 提供构造函数初始化数据
    public ThrustData(int id, int thpower, int topower, string Name, EngineType Type)
    {
        thrustID = id;
        thrustName = Name;
        engineType = Type;
        thrustPower = thpower;
        torqueForcePower = topower;
    }

    // 提供访问器（可选，确保数据安全性）
    public int ThrustID => thrustID;
    public int ThrustPower => thrustPower;
    public int TorqueForcePower => torqueForcePower;
    public string ThrustName => thrustName;
    public EngineType EngineType => engineType;
}

/// <summary>
/// 引擎推力总参数
/// </summary>
public class ParameterEngine : ParameterBase
{
    [Tooltip("对应的推进器ID与对应的推力系数")]
    [SerializeField]
    private SerializableDictionary<string, ThrustData> thrustGroup = new SerializableDictionary<string, ThrustData>();


    /// <summary>
    /// 通过key名自动输入结构体参数的方法
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool TryGetThrustData(string key, out ThrustData data)
    {
        // 检查字典是否初始化，避免空引用
        if (thrustGroup == null)
        {
            data = default;
            return false;
        }
        return thrustGroup.TryGetValue(key, out data);
    }
}
