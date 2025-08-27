using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundthorn : MonoBehaviour
{
    [Header("基础配置")]
    public float demage = 1f;
    [Tooltip("要检测的目标图层（如Player层）")]
    public LayerMask targetLayer;        // 统一使用Layer检测（已修正）
    [Tooltip("伤害冷却时间（秒）")]
    public float damageCooldown = 1f;  // 伤害冷却时间，默认为1秒

    // 状态变量
    private bool isContacting = false;
    private float lastDamageTime = -Mathf.Infinity;  // 上次造成伤害的时间

    void Start()
    {

    }

    void Update()
    {
        // 如果有目标接触且不在冷却中，造成伤害
        if (isContacting &&Time.time - lastDamageTime >= damageCooldown)
        {
            DealDamage();
            lastDamageTime = Time.time;  // 记录本次伤害时间
        }
    }

    // 碰撞检测：通过Layer判断接触（已修正）
    private void OnCollisionEnter2D(Collision2D other)
    {
        // 检查碰撞物体是否在目标图层中
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            isContacting = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // 检查离开物体是否在目标图层中
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            isContacting = false;
        }
    }


    private void DealDamage()
    {
        EventManager.Instance.Emit(new ParameterShipDurability(Durability: -1));
    }
}
