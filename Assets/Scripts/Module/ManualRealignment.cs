using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualRealignment : MonoBehaviour
{
    [Header("UI引用")]
    public Button realignmentButton;

    [Header("引用")]
    public Transform parentTransform;  // 需要回正的父对象Transform

    [Header("设置")]
    public float cooldownTime = 1.0f;  // 冷却时间

    // 私有变量
    private Quaternion initialRotation; // 初始旋转
    private float lastUseTime = -Mathf.Infinity; // 上次使用时间

    void Start()
    {
        // 记录初始旋转
        if (parentTransform != null)
        {
            initialRotation = parentTransform.rotation;
        }

        // 为按钮添加点击事件
        if (realignmentButton != null)
        {
            realignmentButton.onClick.AddListener(Realign);
        }
    }

    void Update()
    {
        // 检查是否在冷却中
        bool isOnCooldown = Time.time < lastUseTime + cooldownTime;

        // 更新按钮状态
        if (realignmentButton != null)
        {
            realignmentButton.interactable = !isOnCooldown;
        }
    }

    // 回正方法（仅调整旋转）
    private void Realign()
    {
        // 检查是否有引用
        if (parentTransform == null)
            return;

        // 检查是否在冷却中
        if (Time.time < lastUseTime + cooldownTime)
            return;

        // 瞬间回正旋转
        parentTransform.rotation = initialRotation;

        // 记录本次使用时间
        lastUseTime = Time.time;
    }
}
