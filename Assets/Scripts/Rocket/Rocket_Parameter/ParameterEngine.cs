using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Engine;


/// <summary>
/// �ƽ��������ṹ
/// </summary>
[System.Serializable]
public struct ThrustData  // �ṹ�彨����Pascal������������ĸ��д��
{
    // �ֶ���Ϊpublic�����[SerializeField]��ȷ������Inspector�б༭
    [Header("ID")][SerializeField] private int thrustID;
    [Header("������")][SerializeField] private string thrustName;
    [Header("��������")][SerializeField] private EngineType engineType;
    [Header("��������")][SerializeField] private int thrustPower;
    [Header("������ת�ٶ�")][SerializeField] private int torqueForcePower;

    // �ṩ���캯����ʼ������
    public ThrustData(int id, int thpower, int topower, string Name, EngineType Type)
    {
        thrustID = id;
        thrustName = Name;
        engineType = Type;
        thrustPower = thpower;
        torqueForcePower = topower;
    }

    // �ṩ����������ѡ��ȷ�����ݰ�ȫ�ԣ�
    public int ThrustID => thrustID;
    public int ThrustPower => thrustPower;
    public int TorqueForcePower => torqueForcePower;
    public string ThrustName => thrustName;
    public EngineType EngineType => engineType;
}

/// <summary>
/// ���������ܲ���
/// </summary>
public class ParameterEngine : ParameterBase
{
    [Tooltip("��Ӧ���ƽ���ID���Ӧ������ϵ��")]
    [SerializeField]
    private SerializableDictionary<string, ThrustData> thrustGroup = new SerializableDictionary<string, ThrustData>();


    /// <summary>
    /// ͨ��key���Զ�����ṹ������ķ���
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool TryGetThrustData(string key, out ThrustData data)
    {
        // ����ֵ��Ƿ��ʼ�������������
        if (thrustGroup == null)
        {
            data = default;
            return false;
        }
        return thrustGroup.TryGetValue(key, out data);
    }
}
