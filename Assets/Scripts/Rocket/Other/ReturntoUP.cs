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
        Debug.Log(PlayerTransform.eulerAngles.z);

        if (Mathf.Abs(PlayerTransform.eulerAngles.z) > retrunAngle)
        {
            Debug.Log("是否触发——角度");
            currentRetrunTimer += Time.deltaTime;
        if (currentRetrunTimer >= retrunTimer)
        {
              // 重置z轴旋转为0（保持x、y轴旋转不变）
              Vector3 currentEuler = PlayerTransform.eulerAngles;
              currentEuler.z = 0; // 只重置z轴
              PlayerTransform.eulerAngles = currentEuler;
              currentRetrunTimer = 0;
        }
    }
        else { currentRetrunTimer = 0; }
    }
}
