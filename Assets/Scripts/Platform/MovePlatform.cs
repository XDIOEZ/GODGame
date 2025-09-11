using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : PlatformBase
{
    [Header("�ƶ�����")]
    public Transform pointA;       // ���
    public Transform pointB;       // �յ�
    public float moveSpeed = 2f;   // �ƶ��ٶ�
    [Range(0.1f, 1f)] public float reachDistance = 0.2f; // �����ж�����
    public bool loopMovement = true; // �Ƿ�ѭ��
    public bool startAtPointA = true; // �Ƿ��A�㿪ʼ

    private Rigidbody2D rb;
    private Vector2 targetPosition; // ʹ��Vector2����2D

    void Awake()
    {
        // ��Ӳ�����2D����
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // 2D���������Ż�
        rb.gravityScale = 0; // ��������
        rb.freezeRotation = true; // ������ת
        rb.mass = 100f; // ���ص�����ʹƽ̨���ȶ�
        rb.drag = 0.5f; // ��΢����ʹ�ƶ���ƽ��
        rb.angularDrag = 0.5f;
    }

    void Start()
    {
        // ��֤���Ƿ�����
        if (pointA == null || pointB == null)
        {
            Debug.LogError("��Ϊ�ƶ�ƽ̨����pointA��pointB��", this);
            enabled = false; // ���ýű��������
            return;
        }

        // ��ʼλ������
        transform.position = startAtPointA ? pointA.position : pointB.position;
        targetPosition = startAtPointA ? pointB.position : pointA.position;
    }

    void FixedUpdate()
    {
        // ���㵽Ŀ���ķ���;���
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

        // ����Ŀ����ж�
        if (distanceToTarget < reachDistance)
        {
            if (loopMovement)
            {
                // �л�Ŀ���
                targetPosition = (targetPosition == (Vector2)pointB.position) ?
                                 pointA.position : pointB.position;
            }
            else
            {
                // ��ѭ��ģʽ��ֹͣ
                rb.velocity = Vector2.zero;
                return;
            }
        }

        // Ӧ��2D�ٶ�
        rb.velocity = direction * moveSpeed;
    }

    // ������ͼ���Ƹ�����
    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);

            // ���������յ��ʶ
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pointA.position, 0.2f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pointB.position, 0.2f);
        }
    }

    // ��д��������÷���
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
