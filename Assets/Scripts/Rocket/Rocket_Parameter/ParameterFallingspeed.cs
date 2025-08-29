using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterFallingspeed : ParameterBase,IEvent
{
    [Header("����ϵ��")]
    [SerializeField]
    private float fallingspeed=10;

    [Header("��ǰ����ϵ��")]
    [SerializeField]
    private float returnCoefficient = 1;

    [Header("��ǰ�������Ť��")]
    [Tooltip("����ܳ���һ�ٰ�ʮ�ȣ���ÿ����ھ�ʮ��")]
    [SerializeField]
    public float maxStabilizationTorque = 10f; // ������Ť�أ��������������
    public float Fallingspeed => fallingspeed;

    public float ReturnCoefficient => returnCoefficient;

    public float MaxStabilizationTorque => maxStabilizationTorque;



    public ParameterFallingspeed(float FallSpeed=1, float returnCff =1)
    {
        fallingspeed = FallSpeed;
        returnCoefficient = returnCff;

    }

    private void OnEnable()
    {
        EventManager.Instance.On<ParameterFallingspeed>(CurrentFallingChange);
    }


    private void OnDisable()
    {
        EventManager.Instance.Off<ParameterFallingspeed>(CurrentFallingChange);
    }


    private void CurrentFallingChange(ParameterFallingspeed evt)
    {
        fallingspeed = evt.fallingspeed;
        returnCoefficient = evt.returnCoefficient;
        Debug.Log($"�ɴ���ǰ����������{evt.fallingspeed}");
        // ʵ����Ŀ�п������ﴦ��UI��ʾ����Ч���š�������µ��߼�
    }
}
