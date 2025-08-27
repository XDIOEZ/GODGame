using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public BoxCollider2D windArea;
    public AreaEffector2D windEffector;

    [Header("风力设置")]
    [Tooltip("风力大小")]
    public float windForce = 20f;
    [Tooltip("风的方向")]
    public Vector2 windDirection = Vector2.right;

    [Header("风力检测层级")]
    [Tooltip("风力检测的LayerMask，需与AreaEffector2D的Force Target Layer一致")]
    public LayerMask windLayerMask;

    [Header("风力区域设置")]
    [Tooltip("风力区域的中心位置（相对于物体）")]
    public Vector2 windAreaOffset = Vector2.zero;

    [Tooltip("风力区域的大小")]
    public Vector2 windAreaSize = new Vector2(5, 5);
   
    void Awake()
    {
        if (windArea == null)
            windArea = GetComponent<BoxCollider2D>();
        if (windEffector == null)
            windEffector = GetComponent<AreaEffector2D>();
        if (windEffector != null)
        {
            windEffector.forceMagnitude = windForce;
            windEffector.forceAngle = Mathf.Atan2(windDirection.y, windDirection.x) * Mathf.Rad2Deg;
            windEffector.forceTarget = EffectorSelection2D.Rigidbody; // 保持默认
            windEffector.colliderMask = windLayerMask;
        }
        if (windArea != null)
        {
            windArea.isTrigger = true;
            windArea.usedByEffector = true;
            windArea.size = windAreaSize;
            windArea.offset = windAreaOffset;
        }
    }

    void Update()
    {
        SetWind(windDirection, windForce);
        SetWindLayerMask(windLayerMask);
        SetWindAreaSize(windAreaSize);
        SetWindAreaPosition((Vector2)transform.position + windAreaOffset);
    }

    public void SetWind(Vector2 direction, float force)
    {
        windDirection = direction.normalized;
        windForce = force;
        if (windEffector != null)
        {
            windEffector.forceMagnitude = windForce;
            windEffector.forceAngle = Mathf.Atan2(windDirection.y, windDirection.x) * Mathf.Rad2Deg;
            windEffector.colliderMask = windLayerMask;
        }
    }

    // 可在外部调用以动态更改风力检测层级
    public void SetWindLayerMask(LayerMask mask)
    {
        windLayerMask = mask;
        if (windEffector != null)
        {
            windEffector.colliderMask = windLayerMask;
        }
    }
    // 新增属性和方法以便外部直接设置风力区域的大小和位置
    public void SetWindAreaSize(Vector2 size)
    {
        if (windArea != null)
        {
            windArea.size = size;
            // AreaEffector2D 依赖于 Collider2D 的范围，无需单独设置
        }
    }

    public void SetWindAreaPosition(Vector2 position)
    {
        if (windArea != null)
        {
            windArea.offset = position - (Vector2)transform.position;
            // 保证碰撞体与物体位置同步
        }
    }

    
}
