using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ParameterShipDurability : ParameterBase,IEvent
{
    [Header("飞船当前最大耐久")]
    [SerializeField]
    private int maxDurability = 10;
    [Header("飞船当前耐久")]
    [SerializeField]
    private int durability=1;
    [Header("当前玩家飞船")]
    public GameObject shipPlayer;

    public float Durability => durability;
    public int MaxDurability => maxDurability;
    public ParameterShipDurability(int Durability)
    {
        durability = Durability;

    }
    private void OnEnable()
    {
        if (durability > maxDurability)
        {
            durability = maxDurability;
        }
        EventManager.Instance.On<ParameterShipDurability>(ChangeDurability);
    }


    private void OnDisable()
    {
        EventManager.Instance.Off<ParameterShipDurability>(ChangeDurability);
    }


    private void ChangeDurability(ParameterShipDurability evt)
    {
        durability += evt.durability;
        Debug.Log($"飞船当前耐久：{durability}");
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
    public void AddDurability(int newDurability)
    {
        durability = newDurability;
    }
}
