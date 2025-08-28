using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRocket : MonoBehaviour
{
    // ���ָ�ֵ
    [Header("���ָ�ֵ")]
    [SerializeField] private int maxRepairValue = 10;
    // ��ǰ�ɱ�ָ���
    [Header("��ǰ�ָ���")]
    [SerializeField] private int currentRepairValue = 1;

    // ֻ����BoxCollider2D
    [Header("ֻ����BoxCollider2D")]
    [SerializeField] private BoxCollider2D targetBoxCollider2D;
    public GameObject shipObject;

    private void OnEnable()
    {
        // ȷ����ǰ�ָ����ں���Χ��
        currentRepairValue = (int)Mathf.Clamp(currentRepairValue, 0, maxRepairValue);
    }

    // �����ײ
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ֻ���ָ����BoxCollider2D����ֻ����һ��
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
                Debug.Log($"��ǰ�;�ֵ: {currentDurability}");
            }
            else
            {
                Debug.LogWarning("δ�ҵ� ParameterShipDurability ���");
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

    // �ⲿ�������ָ�ֵ
    public float MaxRepairValue
    {
        get => maxRepairValue;
        set
        {
            maxRepairValue = (int)value;
        }
    }

    // �ⲿ���ʵ�ǰ�ָ���
    public float CurrentRepairValue
    {
        get => currentRepairValue;
        set => currentRepairValue = (int)Mathf.Clamp(value, 0, maxRepairValue);
    }
    private void DrawRed()
    {
        // �������������Ϊ��ɫ
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
