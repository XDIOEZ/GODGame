using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterFallingspeed : ParameterBase,IEvent
{
    [Header("重力系数")]
    [SerializeField]
    private float fallingspeed=10;

    [Header("当前回正系数")]
    [SerializeField]
    private float returnCoefficient = 1;

    [Header("当前最大修正扭矩")]
    [Tooltip("最大不能超过一百八十度，最好控制在九十度")]
    [SerializeField]
    public float maxStabilizationTorque = 10f; // 最大回正扭矩（避免过度修正）
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
        Debug.Log($"飞船当前所处重力：{evt.fallingspeed}");
        // 实际项目中可在这里处理UI提示、音效播放、任务更新等逻辑
    }
}
