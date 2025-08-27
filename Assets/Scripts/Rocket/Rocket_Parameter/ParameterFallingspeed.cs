using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterFallingspeed : ParameterBase,IEvent
{
    [Header("重力系数")]
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
        Debug.Log($"飞船当前所处重力：{evt.fallingspeed}");
        // 实际项目中可在这里处理UI提示、音效播放、任务更新等逻辑
    }
}
