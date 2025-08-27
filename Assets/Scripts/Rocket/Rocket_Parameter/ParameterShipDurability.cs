using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ParameterShipDurability : ParameterBase,IEvent
{
    [Header("�ɴ���ǰ�;�")]
    [SerializeField]
    private int durability=10;
    [Header("��ǰ��ҷɴ�")]
    public GameObject shipPlayer;

    public float Durability => durability;

    public ParameterShipDurability(int Durability)
    {
        durability = Durability;

    }
    private void OnEnable()
    {
        EventManager.Instance.On<ParameterShipDurability>(ChangeDurability);
    }


    private void OnDisable()
    {
        EventManager.Instance.Off<ParameterShipDurability>(ChangeDurability);
    }


    private void ChangeDurability(ParameterShipDurability evt)
    {
        durability += evt.durability;
        Debug.Log($"�ɴ���ǰ�;ã�{durability}");
        AdjustDurability();
    }


    private bool AdjustDurability()
    {
        if (durability >=1) { return true; }
        else 
        {
            DestroySelf();
            return false;
        }
    }

    private void DestroySelf()
    {
        Destroy(shipPlayer.gameObject);
    }
}
