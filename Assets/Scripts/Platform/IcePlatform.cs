using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatform : PlatformBase
{
    [Header("检测配置")]
    public float checkRadius = 1f;          // 检测范围（球形用半径，矩形用边长）
    public LayerMask targetLayer;           // 要检测的物体层（如 Player）
    public bool isSphere = false;           // 是否是球形平台（否则视为矩形）

    [Header("缩放配置")]
    public float scaleSpeed = 0.1f;         // 缩放速度
    public Vector3 scaleAxis = new Vector3(1, 1, 1); // 缩放轴（x/y/z控制，1=缩放，0=不缩放）
    public Vector3 minScale = new Vector3(0.1f, 0.1f, 1); // 最小缩放限制

    private Vector3 initialScale;           // 初始缩放
    private bool hasTarget = false;         // 是否检测到物体

    void Start()
    {
        initialScale = transform.localScale; // 记录初始缩放
    }

    void Update()
    {
        // 检测平台上方是否有物体
        hasTarget = CheckAboveObject();

        if (hasTarget)
        {
            // 有物体：逐渐缩小
            ScalePlatform(false);
        }
        else
        {
            // 无物体：恢复缩放
            ScalePlatform(true);
        }
    }

    // 检测平台上方是否有物体
    private bool CheckAboveObject()
    {
        if (isSphere)
        {
            // 球形检测：用圆形检测
            return Physics2D.OverlapCircle(transform.position, checkRadius, targetLayer);
        }
        else
        {
            // 矩形检测：用矩形区域检测
            Vector2 size = new Vector2(
                transform.localScale.x * checkRadius,
                transform.localScale.y * checkRadius
            );
            return Physics2D.OverlapArea(
                (Vector2)transform.position - size / 2,
                (Vector2)transform.position + size / 2,
                targetLayer
            );
        }
    }

    // 缩放平台（isRecover：是否恢复）
    private void ScalePlatform(bool isRecover)
    {
        Vector3 targetScale = isRecover ? initialScale : minScale;

        // 按轴缩放（scaleAxis 控制是否允许缩放）
        for (int i = 0; i < 3; i++)
        {
            if (scaleAxis[i] == 0)
            {
                // 禁止缩放的轴，锁定为初始值
                targetScale[i] = initialScale[i];
            }
        }

        // 平滑过渡缩放
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            scaleSpeed * Time.deltaTime
        );
    }

    // Gizmos 辅助调试（可选）
    void OnDrawGizmos()
    {
        Gizmos.color = hasTarget ? Color.red : Color.green;
        if (isSphere)
        {
            Gizmos.DrawWireSphere(transform.position, checkRadius);
        }
        else
        {
            Vector2 size = new Vector2(
                transform.localScale.x * checkRadius,
                transform.localScale.y * checkRadius
            );
            Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 1));
        }
    }
}
