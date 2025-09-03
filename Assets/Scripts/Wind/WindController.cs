using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{   
    public BoxCollider2D windArea;      
    public AreaEffector2D windEffector;

    [Header("��������")]
    [Tooltip("������С")]
    public float windForce = 20f;
    [Tooltip("��ķ���")]
    public Vector2 windDirection = Vector2.right;

    [Header("�������㼶")]
    [Tooltip("��������LayerMask������AreaEffector2D��Force Target Layerһ��")]
    public LayerMask windLayerMask;

    [Header("������������")]
    [Tooltip("�������������λ�ã���������壩")]
    public Vector2 windAreaOffset = Vector2.zero;

    [Tooltip("��������Ĵ�С")]
    public Vector2 windAreaSize = new Vector2(5, 5);

    public Transform windParticleTransform; // ��ק������������嵽��

    [Header("��������Ч������")]
    [Tooltip("����Ч���������С��Shapeģ���scale��")]
    public Vector2 particleAreaSize = new Vector2(5, 5);

    [Tooltip("����Ч��������ƫ�ƣ�����ڷ������ģ�")]
    public Vector2 particleAreaOffset = Vector2.zero;

    [Header("�����������ó�������")]
    [Tooltip("�����ٶȣ�Ӱ�����ó��ȣ�")]
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

                // ����������ȣ������������ϵ�ͶӰ���ȣ�
                Vector2 dir = windDirection.normalized;
                float areaLength = Mathf.Abs(windAreaSize.x * dir.x) + Mathf.Abs(windAreaSize.y * dir.y);

                // ��ֹ����0
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
        // ������������
        Vector3 areaCenter = transform.TransformPoint(windAreaOffset);
        Vector3 areaSize = new Vector3(windAreaSize.x, windAreaSize.y, 0.1f);

        // ���浱ǰGizmos����
        Matrix4x4 oldMatrix = Gizmos.matrix;
        // ����Gizmos����ʹ�����������ת
        Gizmos.matrix = Matrix4x4.TRS(areaCenter, transform.rotation, Vector3.one);

        // ���Ʒ���
        Gizmos.color = new Color(0.3f, 0.7f, 1f, 0.3f);
        Gizmos.DrawCube(Vector3.zero, areaSize);
        Gizmos.color = new Color(0.3f, 0.7f, 1f, 1f);
        Gizmos.DrawWireCube(Vector3.zero, areaSize);

        // ���Ʒ��������ͷ
        Vector3 windDir3D = (Quaternion.Euler(0, 0, transform.eulerAngles.z) * new Vector3(windDirection.x, windDirection.y, 0)).normalized;
        float arrowLength = Mathf.Min(windAreaSize.x, windAreaSize.y) * 0.4f;
        Vector3 arrowStart = Vector3.zero;
        Vector3 arrowEnd = arrowStart + windDir3D * arrowLength;
        Gizmos.DrawLine(arrowStart, arrowEnd);

        Vector3 right = Quaternion.AngleAxis(150, Vector3.forward) * (arrowEnd - arrowStart).normalized * 0.3f;
        Vector3 left = Quaternion.AngleAxis(-150, Vector3.forward) * (arrowEnd - arrowStart).normalized * 0.3f;
        Gizmos.DrawLine(arrowEnd, arrowEnd + right);
        Gizmos.DrawLine(arrowEnd, arrowEnd + left);

        // �ָ�Gizmos����
        Gizmos.matrix = oldMatrix;
    }
#endif
}
