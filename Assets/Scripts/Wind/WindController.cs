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
            windEffector.forceTarget = EffectorSelection2D.Rigidbody; // ����Ĭ��
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

    // �����ⲿ�����Զ�̬���ķ������㼶
    public void SetWindLayerMask(LayerMask mask)
    {
        windLayerMask = mask;
        if (windEffector != null)
        {
            windEffector.colliderMask = windLayerMask;
        }
    }
    // �������Ժͷ����Ա��ⲿֱ�����÷�������Ĵ�С��λ��
    public void SetWindAreaSize(Vector2 size)
    {
        if (windArea != null)
        {
            windArea.size = size;
            // AreaEffector2D ������ Collider2D �ķ�Χ�����赥������
        }
    }

    public void SetWindAreaPosition(Vector2 position)
    {
        if (windArea != null)
        {
            windArea.offset = position - (Vector2)transform.position;
            // ��֤��ײ��������λ��ͬ��
        }
    }

    
}
