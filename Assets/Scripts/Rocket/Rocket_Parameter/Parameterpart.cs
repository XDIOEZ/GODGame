using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterPart : MonoBehaviour
{
    [Header("��������")]
    public float magnetRange = 5f;          // ����������Χ
    public float magnetForce = 20f;         // ������ǿ��
    public float maxMagnetSpeed = 3f;       // ��������ٶ�
    public float minDistanceToStop = 0.5f;  // ��Сֹͣ����

    [Header("����")]
    public Transform playerTarget;          // ��ҵ�Transform
    private Rigidbody2D rb;                 // ��Դ�����Rigidbody2D

    [Header("Ҫ����Ŀ��ͼ��")]
    public LayerMask targetLayer;

    [Header("��Ҫ�󶨵ķɴ���ǩ��")]
    [SerializeField]
    private string playerName = "Player";

    private bool isInMagnetRange = false;   // �Ƿ���������Χ��


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // ��ʼ��Rigidbody2D״̬��ȷ����������Ӱ���ҳ�ʼ��ֹ
        if (rb != null)
        {
            rb.gravityScale = 0;            // ��������
            rb.velocity = Vector2.zero;     // ��ʼ�ٶ���Ϊ0
            rb.angularVelocity = 0;         // ��ʼ���ٶ���Ϊ0
        }

        // �Զ��ҵ����
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerName);
            if (player != null)
                playerTarget = player.transform;
            else
                Debug.LogError("δ�ҵ���ң���������ȷ�ı�ǩ���ֶ���ֵ");
        }
    }

    private void Update()
    {
        if (playerTarget == null || !CanMagnet())
        {
            isInMagnetRange = false;
            return;
        }

        // ���㵽��ҵľ��벢��������״̬
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);
        isInMagnetRange = distanceToPlayer <= magnetRange && distanceToPlayer > minDistanceToStop;

        // ����������Χʱ��ȷ�����徲ֹ
        if (!isInMagnetRange && rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
    }


    private void FixedUpdate()
    {
        if (!isInMagnetRange || rb == null)
            return;

        // ������������
        Vector2 magnetDirection = (playerTarget.position - transform.position).normalized;

        // ʩ��������
        rb.AddForce(magnetDirection * magnetForce * Time.fixedDeltaTime, ForceMode2D.Force);

        // ��������ٶ�
        if (rb.velocity.magnitude > maxMagnetSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxMagnetSpeed;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            EventManager.Instance.Emit(new ParameterPartsTouchEvent());
            Destroy(this.gameObject);
        }
    }

    private bool CanMagnet()
    {
        return gameObject.activeSelf;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistanceToStop);
    }
}
