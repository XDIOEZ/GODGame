using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPlatform : PlatformBase
{
    [Header("基础配置")]
    public float disappearDuration = 3f; // 消失所需接触时间
    public float fadeSpeed = 2f;         // 渐变速度
    [Tooltip("要检测的目标图层（如Player层）")]
    public LayerMask targetLayer;        // 统一使用Layer检测

    [Header("恢复设置")]
    [Tooltip("是否允许离开后恢复原状（消失前）")]
    public bool shouldRecover = true;    // 消失前是否恢复

    [Tooltip("是否允许完全消失后重生")]
    public bool canRecover = true;       // 完全消失后是否允许重生

    [Header("虚化掉落设置")]
    public bool canFall = true;       // 是否允许中间掉落
    [Tooltip("最大值为1，最小值为0")]
    public float canFallTimeFactor = 0.5f;
    private float FallTimeAdjust;
    
    [Header(("重生所需时间"))]
    public float recoverDuration = 5f;   // 从消失到开始重生的间隔时间

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;

    // 状态变量
    private bool isContacting = false;   // 是否有物体接触
    private float contactTimer = 0f;     // 接触计时
    private float targetAlpha = 1f;      // 目标透明度
    private bool isDisappeared = false;  // 是否已消失
    private float recoverTimer = 0f;     // 重生计时器

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        SetAlpha(1f); // 初始完全显示
    }

    void Update()
    {
        if (isDisappeared)
        {
            // 如果允许重生，进入重生倒计时
            if (canRecover)
            {
                recoverTimer += Time.deltaTime;
                if (recoverTimer >= recoverDuration)
                {
                    // 重生逻辑
                    Recover();
                }
            }
        }
        else if (isContacting)
        {
            // 接触时：计时并逐渐消失
            contactTimer += Time.deltaTime;
            targetAlpha = 1 - (contactTimer / disappearDuration);
            FallTimeAdjust = canFallTimeFactor * disappearDuration;
            if (canFall)
            {
                if (contactTimer >= FallTimeAdjust)
                {
                    isDisappeared = true;
                    platformCollider.enabled = false; // 禁用碰撞
                }
            }
            if (contactTimer >= disappearDuration)
            {
                    isDisappeared = true;
                    platformCollider.enabled = false; // 禁用碰撞
            }
        }
        else
        {
            // 离开时：根据开关决定是否恢复（消失前）
            if (shouldRecover)
            {
                // 允许恢复：逐渐变回不透明
                targetAlpha += Time.deltaTime * fadeSpeed;
                targetAlpha = Mathf.Clamp01(targetAlpha);

                // 完全恢复后重置计时
                if (targetAlpha >= 1f)
                {
                    contactTimer = 0f;
                }
            }
            // 不允许恢复：保持当前透明度和计时
        }



        // 应用透明度渐变
        float currentAlpha = Mathf.MoveTowards(
            spriteRenderer.color.a,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );
        SetAlpha(currentAlpha);
    }

    // 重生方法：重置状态
    private void Recover()
    {
        isDisappeared = false;
        platformCollider.enabled = true;
        targetAlpha = 1f;
        contactTimer = 0f;
        recoverTimer = 0f;
    }

    // 碰撞检测：通过Layer判断接触
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

    // 设置透明度
    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    // 外部控制接口：动态修改是否允许恢复
    public void SetShouldRecover(bool value)
    {
        shouldRecover = value;
    }

    // 外部控制接口：动态修改是否允许重生
    public void SetCanRecover(bool value)
    {
        canRecover = value;
    }
}
