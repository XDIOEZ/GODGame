using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundthorn : MonoBehaviour
{
    [Header("基础配置")]
    public int demage = 1;
    [Tooltip("要检测的目标图层（如Player层）")]
    public LayerMask targetLayer;        // 统一使用Layer检测（已修正）
    [Tooltip("伤害冷却时间（秒）")]
    public float damageCooldown = 1f;  // 伤害冷却时间，默认为1秒

    // 核心状态：仅由计时器控制的冷却标记
    private bool _isInCooldown;
    // 计时器ID：仅用于销毁时清理，不参与冷却逻辑
    private string _cooldownTimerId;

    void Start()
    {

    }

    void Update()
    {

    }

    // 碰撞检测：通过Layer判断接触（已修正）
    private void OnCollisionStay2D(Collision2D other)
    {
        // 检查碰撞物体是否在目标图层中
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            DealDamage();
        }
    }

    private void OnDisable()
    {

    }
    private void DealDamage()
    {
        Debug.Log(_isInCooldown);
        if (!_isInCooldown)
        {
            EventManager.Instance.Emit(new ParameterShipDurability(Durability: -demage));
            StartCooldown();
        }
    }

    private void StartCooldown()
    {
        _isInCooldown = true;

        // 先清除旧计时器（避免重复计时）
        if (!string.IsNullOrEmpty(_cooldownTimerId))
        {
            TimerManager.Instance.RemoveTimer(_cooldownTimerId);
        }
        // 延迟 2 秒后执行回调
        _cooldownTimerId = TimerManager.Instance.AddTimer(damageCooldown, () =>
        {
            Debug.Log("计时器回调触发！当前时间：" + Time.time); // 新增这行，确保独立执行
            _isInCooldown = false;
            _cooldownTimerId = null; // 清空ID，避免残留
            // 可在这里写：播放动画、发送网络请求、刷新 UI 等
        });
    }
}
