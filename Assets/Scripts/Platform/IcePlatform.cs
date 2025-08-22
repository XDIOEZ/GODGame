using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatform : PlatformBase
{
    [Header("�������")]
    public float checkRadius = 1f;          // ��ⷶΧ�������ð뾶�������ñ߳���
    public LayerMask targetLayer;           // Ҫ��������㣨�� Player��
    public bool isSphere = false;           // �Ƿ�������ƽ̨��������Ϊ���Σ�

    [Header("��������")]
    public float scaleSpeed = 0.1f;         // �����ٶ�
    public Vector3 scaleAxis = new Vector3(1, 1, 1); // �����ᣨx/y/z���ƣ�1=���ţ�0=�����ţ�
    public Vector3 minScale = new Vector3(0.1f, 0.1f, 1); // ��С��������

    private Vector3 initialScale;           // ��ʼ����
    private bool hasTarget = false;         // �Ƿ��⵽����

    void Start()
    {
        initialScale = transform.localScale; // ��¼��ʼ����
    }

    void Update()
    {
        // ���ƽ̨�Ϸ��Ƿ�������
        hasTarget = CheckAboveObject();

        if (hasTarget)
        {
            // �����壺����С
            ScalePlatform(false);
        }
        else
        {
            // �����壺�ָ�����
            ScalePlatform(true);
        }
    }

    // ���ƽ̨�Ϸ��Ƿ�������
    private bool CheckAboveObject()
    {
        if (isSphere)
        {
            // ���μ�⣺��Բ�μ��
            return Physics2D.OverlapCircle(transform.position, checkRadius, targetLayer);
        }
        else
        {
            // ���μ�⣺�þ���������
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

    // ����ƽ̨��isRecover���Ƿ�ָ���
    private void ScalePlatform(bool isRecover)
    {
        Vector3 targetScale = isRecover ? initialScale : minScale;

        // �������ţ�scaleAxis �����Ƿ��������ţ�
        for (int i = 0; i < 3; i++)
        {
            if (scaleAxis[i] == 0)
            {
                // ��ֹ���ŵ��ᣬ����Ϊ��ʼֵ
                targetScale[i] = initialScale[i];
            }
        }

        // ƽ����������
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            scaleSpeed * Time.deltaTime
        );
    }

    // Gizmos �������ԣ���ѡ��
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
