using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRocket : MonoBehaviour
{
    // 最大恢复值
    [Header("最大恢复值")]
    [SerializeField] private int maxRepairValue = 10;
    // 当前可变恢复量
    [Header("当前恢复量")]
    [SerializeField] private int currentRepairValue = 1;

    // 只检测此BoxCollider2D
    [Header("只检测此BoxCollider2D")]
    [SerializeField] private BoxCollider2D targetBoxCollider2D;
    public GameObject shipObject;

    private void OnEnable()
    {
        // 确保当前恢复量在合理范围内
        currentRepairValue = (int)Mathf.Clamp(currentRepairValue, 0, maxRepairValue);
    }

    // 检测碰撞
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只检测指定的BoxCollider2D，且只触发一次
        if (!hasTriggered && targetBoxCollider2D != null && other == targetBoxCollider2D)
        {
            hasTriggered = true;
            Debug.Log("FixRocket: Detected collision with target BoxCollider2D.");
            DrawRed();
            var durabilityComponent = shipObject.GetComponent<ParameterShipDurability>();
            if (durabilityComponent != null)
            {
                float currentDurability = durabilityComponent.Durability;
                currentDurability = currentDurability + currentRepairValue;

                durabilityComponent.AddDurability((int)currentRepairValue);
                if (currentDurability >= durabilityComponent.MaxDurability)
                {
                    currentDurability = durabilityComponent.MaxDurability;
                }
                Debug.Log($"当前耐久值: {currentDurability}");
            }
            else
            {
                Debug.LogWarning("未找到 ParameterShipDurability 组件");
            }
        }
    }

    private void OnDisable()
    {
        hasTriggered = false;
    }
    // Update is called once per frame
    void Update()
    {

    }

    // 外部访问最大恢复值
    public float MaxRepairValue
    {
        get => maxRepairValue;
        set
        {
            maxRepairValue = (int)value;
        }
    }

    // 外部访问当前恢复量
    public float CurrentRepairValue
    {
        get => currentRepairValue;
        set => currentRepairValue = (int)Mathf.Clamp(value, 0, maxRepairValue);
    }
    private void DrawRed()
    {
        // 将所有子物体变为红色
        foreach (Transform child in transform)
        {
            var renderer = child.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = Color.red;
            }
        }
    }
    
}
