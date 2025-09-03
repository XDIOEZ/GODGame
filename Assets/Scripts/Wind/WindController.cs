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

    public Transform windParticleTransform; // 拖拽你的粒子子物体到此

    [Header("风力粒子效果设置")]
    [Tooltip("粒子效果的区域大小（Shape模块的scale）")]
    public Vector2 particleAreaSize = new Vector2(5, 5);

    [Tooltip("粒子效果的中心偏移（相对于风区中心）")]
    public Vector2 particleAreaOffset = Vector2.zero;

    [Header("风力粒子作用长度设置")]
    [Tooltip("粒子速度（影响作用长度）")]
    public float particleStartSpeed = 5f;
  
    private ParticleSystem windParticleSystem;

    private void OnValidate()
    {
        if (windEffector == null)
            windEffector = GetComponent<AreaEffector2D>();
        if (windArea == null)
            windArea = GetComponent<BoxCollider2D>();

        ApplyWindSettings();
        ApplyWindAreaSettings();
    }

    private void Reset()
    {
        windEffector = GetComponent<AreaEffector2D>();
        windArea = GetComponent<BoxCollider2D>();
        ApplyWindSettings();
        ApplyWindAreaSettings();
    }

    private void Awake()
    {
        ApplyWindSettings();
        ApplyWindAreaSettings();

        if (windParticleTransform != null)
            windParticleSystem = windParticleTransform.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.SceneView.RepaintAll();
        }
#endif
    }

    private void ApplyWindSettings()
    {
        if (windEffector != null)
        {
            windEffector.forceMagnitude = windForce;
            windEffector.forceAngle = Mathf.Atan2(windDirection.y, windDirection.x) * Mathf.Rad2Deg;
            windEffector.colliderMask = windLayerMask;
        }
    }

    private void ApplyWindAreaSettings()
    {
        if (windArea != null)
        {
            windArea.isTrigger = true;
            windArea.usedByEffector = true;
            windArea.size = windAreaSize;
            windArea.offset = windAreaOffset;
        }

        if (windParticleTransform != null)
        {
            Vector3 worldCenter = transform.TransformPoint(windAreaOffset + particleAreaOffset);
            windParticleTransform.position = worldCenter;
            windParticleTransform.rotation = transform.rotation;

            if (windParticleSystem == null)
                windParticleSystem = windParticleTransform.GetComponent<ParticleSystem>();

            if (windParticleSystem != null)
            {
                var shape = windParticleSystem.shape;
                shape.scale = new Vector3(particleAreaSize.x, particleAreaSize.y, 1f);

                // 计算风区长度（风向在区域上的投影长度）
                Vector2 dir = windDirection.normalized;
                float areaLength = Mathf.Abs(windAreaSize.x * dir.x) + Mathf.Abs(windAreaSize.y * dir.y);

                // 防止除以0
                float speed = Mathf.Max(0.01f, particleStartSpeed);
                float lifetime = areaLength / speed;

                var main = windParticleSystem.main;
                main.startSpeed = particleStartSpeed;
                main.startLifetime = lifetime;
            }
        }
    }

    public void SetWind(Vector2 direction, float force)
    {
        windDirection = direction.normalized;
        windForce = force;
        ApplyWindSettings();
    }

    public void SetWindLayerMask(LayerMask mask)
    {
        windLayerMask = mask;
        ApplyWindSettings();
    }

    public void SetWindAreaSize(Vector2 size)
    {
        windAreaSize = size;
        ApplyWindAreaSettings();
    }

    public void SetWindAreaPosition(Vector2 position)
    {
        windAreaOffset = position - (Vector2)transform.position;
        ApplyWindAreaSettings();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // 计算区域中心
        Vector3 areaCenter = transform.TransformPoint(windAreaOffset);
        Vector3 areaSize = new Vector3(windAreaSize.x, windAreaSize.y, 0.1f);

        // 保存当前Gizmos矩阵
        Matrix4x4 oldMatrix = Gizmos.matrix;
        // 设置Gizmos矩阵，使其跟随物体旋转
        Gizmos.matrix = Matrix4x4.TRS(areaCenter, transform.rotation, Vector3.one);

        // 绘制风区
        Gizmos.color = new Color(0.3f, 0.7f, 1f, 0.3f);
        Gizmos.DrawCube(Vector3.zero, areaSize);
        Gizmos.color = new Color(0.3f, 0.7f, 1f, 1f);
        Gizmos.DrawWireCube(Vector3.zero, areaSize);

        // 绘制风力方向箭头
        Vector3 windDir3D = (Quaternion.Euler(0, 0, transform.eulerAngles.z) * new Vector3(windDirection.x, windDirection.y, 0)).normalized;
        float arrowLength = Mathf.Min(windAreaSize.x, windAreaSize.y) * 0.4f;
        Vector3 arrowStart = Vector3.zero;
        Vector3 arrowEnd = arrowStart + windDir3D * arrowLength;
        Gizmos.DrawLine(arrowStart, arrowEnd);

        Vector3 right = Quaternion.AngleAxis(150, Vector3.forward) * (arrowEnd - arrowStart).normalized * 0.3f;
        Vector3 left = Quaternion.AngleAxis(-150, Vector3.forward) * (arrowEnd - arrowStart).normalized * 0.3f;
        Gizmos.DrawLine(arrowEnd, arrowEnd + right);
        Gizmos.DrawLine(arrowEnd, arrowEnd + left);

        // 恢复Gizmos矩阵
        Gizmos.matrix = oldMatrix;
    }
#endif
}
