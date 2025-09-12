using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : PlatformBase
{
    [Header("移动设置")]
    public Transform pointA;       // 起点
    public Transform pointB;       // 终点
    public float moveSpeed = 2f;   // 移动速度
    [Range(0.1f, 1f)] public float reachDistance = 0.2f; // 到达判定距离
    public bool loopMovement = true; // 是否循环
    public bool startAtPointA = true; // 是否从A点开始

    private Rigidbody2D rb;
    private Vector2 targetPosition; // 使用Vector2适配2D

    void Awake()
    {
        // 添加并配置2D刚体
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // 2D物理配置优化
        rb.gravityScale = 0; // 禁用重力
        rb.freezeRotation = true; // 冻结旋转
        rb.mass = 100f; // 较重的质量使平台更稳定
        rb.drag = 0.5f; // 轻微阻力使移动更平滑
        rb.angularDrag = 0.5f;
    }

    void Start()
    {
        // 验证点是否设置
        if (pointA == null || pointB == null)
        {
            Debug.LogError("请为移动平台设置pointA和pointB！", this);
            enabled = false; // 禁用脚本避免错误
            return;
        }

        // 初始位置设置
        transform.position = startAtPointA ? pointA.position : pointB.position;
        targetPosition = startAtPointA ? pointB.position : pointA.position;
    }

    void FixedUpdate()
    {
        // 计算到目标点的方向和距离
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

        // 到达目标点判断
        if (distanceToTarget < reachDistance)
        {
            if (loopMovement)
            {
                // 切换目标点
                targetPosition = (targetPosition == (Vector2)pointB.position) ?
                                 pointA.position : pointB.position;
            }
            else
            {
                // 非循环模式下停止
                rb.velocity = Vector2.zero;
                return;
            }
        }

        // 应用2D速度
        rb.velocity = direction * moveSpeed;
    }

    // 场景视图绘制辅助线
    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);

            // 绘制起点和终点标识
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pointA.position, 0.2f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointB.position, 0.2f);
        }
    }

    // 重写基类的重置方法
    public  void ResetPlatform()
    {
        transform.position = startAtPointA ? pointA.position : pointB.position;
        targetPosition = startAtPointA ? pointB.position : pointA.position;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
