using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : PlatformBase
{
    [Header("移动配置")]
    public Transform pointA;       // A点引用（ Inspector 拖拽赋值 ）
    public Transform pointB;       // B点引用（ Inspector 拖拽赋值 ）
    public float moveSpeed = 2f;   // 平台移动速度
    public bool isLooping = true;  // 是否循环往返

    [Header("持久化配置")]
    public bool usePersistence = true; // 是否启用持久化记录
    private Vector3 initialPosition;   // 初始位置（用于恢复）
    private float savedProgress = 0f;  // 持久化保存的移动进度

    // 运行时状态
    private float moveProgress = 0f;   // 移动进度（0~1 归一化）
    private int moveDirection = 1;     // 移动方向（1=正向，-1=反向）

    void Start()
    {
        // 初始化持久化数据
        if (usePersistence)
        {
            // 从 PlayerPrefs 读取上次销毁时保存的进度
            savedProgress = PlayerPrefs.GetFloat("MovablePlatformProgress", 0f);
            moveProgress = savedProgress;
        }

        // 记录初始位置（用于重置）
        initialPosition = transform.position;
        UpdatePlatformPosition(); // 应用初始进度
    }

    void Update()
    {
        // 更新移动进度
        moveProgress += moveDirection * moveSpeed * Time.deltaTime;
        moveProgress = Mathf.Clamp(moveProgress, 0f, 1f);

        // 到达端点时转向
        if (isLooping)
        {
            if (moveProgress >= 1f || moveProgress <= 0f)
            {
                moveDirection *= -1; // 反向
            }
        }
        else
        {
            // 非循环模式：到达端点后停止
            moveProgress = Mathf.Clamp(moveProgress, 0f, 1f);
            if (moveProgress >= 1f || moveProgress <= 0f)
            {
                moveDirection = 0; // 停止移动
            }
        }

        // 更新平台位置
        UpdatePlatformPosition();
    }

    /// <summary>
    /// 根据归一化进度更新平台位置
    /// </summary>
    private void UpdatePlatformPosition()
    {
        // 计算A→B的插值位置
        Vector3 targetPos = Vector3.Lerp(pointA.position, pointB.position, moveProgress);
        transform.position = targetPos;
    }

    /// <summary>
    /// 持久化记录：销毁时保存当前进度
    /// </summary>
    private void OnDestroy()
    {
        if (usePersistence)
        {
            PlayerPrefs.SetFloat("MovablePlatformProgress", moveProgress);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 外部接口：重置平台到初始状态
    /// </summary>
    public void ResetPlatform()
    {
        transform.position = initialPosition;
        moveProgress = 0f;
        moveDirection = 1;

        // 清除持久化数据（可选）
        if (usePersistence)
        {
            PlayerPrefs.DeleteKey("MovablePlatformProgress");
            PlayerPrefs.Save();
        }
    }

    // 调试用：在Scene视图显示A、B点
    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.1f);
            Gizmos.DrawSphere(pointB.position, 0.1f);
        }
    }
}
