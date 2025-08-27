using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterFallingspeed : ParameterBase,IEvent
{
    [Header("����ϵ��")]
    [SerializeField]
    private float fallingspeed=10;


    public float Fallingspeed => fallingspeed;
    public ParameterFallingspeed(float FallSpeed)
    {
        fallingspeed = FallSpeed;

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
        Debug.Log($"�ɴ���ǰ����������{evt.fallingspeed}");
        // ʵ����Ŀ�п������ﴦ��UI��ʾ����Ч���š�������µ��߼�
    }
}
