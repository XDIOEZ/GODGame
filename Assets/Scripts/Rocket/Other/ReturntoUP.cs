using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReturntoUP : MonoBehaviour
{
    [Header("需要绑定的飞船标签（tag）名")]
    [SerializeField]
    private string playerName;

    [Header("要检测的目标图层（如Ground层）")]
    public LayerMask targetLayer;        // 统一使用Layer检测（已修正）

    [Header("回正最大忍耐时间")]
    [SerializeField]
    public float retrunTimer=1f;        // 统一使用Layer检测（已修正）


    [Header("回正检测最大夹角")]
    [Tooltip("只能0到180度之间")]
    [SerializeField]
    private float retrunAngle = 0;        // 统一使用Layer检测（已修正）

    [Header("燃料组件")]
    public ParameterFuel fuel;        // 统一使用Layer检测（已修正）

    [Header("平滑旋转参数")]
    public float smoothRotationTime = 0.5f; // 平滑旋转所需时间
    private bool isSmoothingRotation = false; // 平滑旋转状态标志
    private float currentReturnTimer = 0; // 平滑旋转计时器

    [SerializeField]
    private float currentRetrunTimer = 0f;        // 统一使用Layer检测（已修正）


    [SerializeField]
    private Transform PlayerTransform;
    // Start is called before the first frame update
    void Start()
    {
        // 自动找到玩家（如果没手动拖，根据标签找，玩家标签设为"Player"）
        if (PlayerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerName);
            if (player != null)
                PlayerTransform = player.transform;
            else
                Debug.LogError("未找到玩家！请给玩家设置标签'Player'，或手动赋值playerTarget");
        }
    }

    private void Update()
    {
        if (fuel.fuel <= 0)
        {
            ReturnShopLowBool();
        }
        
    }

    private void FixedUpdate()
    {
        //ReturnShopLow();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            CheckTimerReturn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentRetrunTimer = 0;
    }


    private void CheckTimerReturn()
    {
        //Debug.Log(PlayerTransform.eulerAngles.z);

        if (Mathf.Abs(PlayerTransform.eulerAngles.z) > retrunAngle)
        {
            //Debug.Log("是否触发——角度");
            currentRetrunTimer += Time.deltaTime;
        if (currentRetrunTimer >= retrunTimer)
        {
                ReturnShopRight();
        }
    }
        else { currentRetrunTimer = 0; }
    }

    private void ReturnShopRight()
    {
        // 重置z轴旋转为0（保持x、y轴旋转不变）
        Vector3 currentEuler = PlayerTransform.eulerAngles;
        currentEuler.z = 0; // 只重置z轴
        PlayerTransform.eulerAngles = currentEuler;
        currentRetrunTimer = 0;
    }

    // 平滑过渡将Z轴旋转回正
    private void ReturnShopLowBool()
    {
        isSmoothingRotation = true; // 启用平滑旋转标志
        currentReturnTimer = 0; // 重置计时器
    }
    private void ReturnShopLow()
    {
        // 如果需要平滑旋转，在Update中逐步过渡
        if (isSmoothingRotation)
        {
            currentReturnTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(currentReturnTimer / smoothRotationTime);

            // 获取当前旋转并只平滑Z轴
            Vector3 currentEuler = PlayerTransform.eulerAngles;
            // 使用Lerp进行平滑过渡，从当前Z值过渡到0
            currentEuler.z = Mathf.LerpAngle(currentEuler.z, 0, progress);
            PlayerTransform.eulerAngles = currentEuler;

            // 当旋转完成后停止平滑
            if (progress >= 1f)
            {
                isSmoothingRotation = false;
                currentEuler.z = 0; // 确保最终精确到0
                PlayerTransform.eulerAngles = currentEuler;
            }
        }
    }

    
}
