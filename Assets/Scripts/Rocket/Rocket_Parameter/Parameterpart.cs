using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterPart : MonoBehaviour
{
    [Header("��������")]
    public float magnetRange = 5f;          // ����������Χ����ҽ��������Χ��ʼ������
    public float magnetForce = 20f;         // ������ǿ�ȣ�Խ������Խ�죬����5-30���ԣ�
    public float maxMagnetSpeed = 3f;       // ��������ٶȣ�������Դ��̫�죬����2-5��
    public float minDistanceToStop = 0.5f;  // ��Сֹͣ���루�����С���������ʱֹͣ��������������

    [Header("����")]
    public Transform playerTarget;          // ��ҵ�Transform������Inspector�ϣ����Զ��ң�
    private Rigidbody2D rb;                 // ��Դ�����Rigidbody2D����������������

    [Header("Ҫ����Ŀ��ͼ�㣨��Player�㣩")]
    public LayerMask targetLayer;        // ͳһʹ��Layer��⣨��������

    [Header("��Ҫ�󶨵ķɴ���ǩ��tag����")]
    [SerializeField]
    private string playerName;

    private bool isInMagnetRange = false;   // �Ƿ���������Χ��


    private void Awake()
    {
        // �Զ���ȡ��Դ��Rigidbody2D��ȷ����Դ���������������ò�ֵ�ƶ���
        rb = GetComponent<Rigidbody2D>();

        // �Զ��ҵ���ң����û�ֶ��ϣ����ݱ�ǩ�ң���ұ�ǩ��Ϊ"Player"��
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerName);
            if (player != null)
                playerTarget = player.transform;
            else
                Debug.LogError("δ�ҵ���ң����������ñ�ǩ'Player'�����ֶ���ֵplayerTarget");
        }
    }
    private void Update()
    {
        // ���ж�����Ƿ���ڣ��Լ���Դ�Ƿ���Ҫ����
        if (playerTarget == null || !CanMagnet())
        {
            isInMagnetRange = false;
            return;
        }

        // 1. ������Դ����ҵľ���
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        // 2. �ж��Ƿ����������Χ
        isInMagnetRange = distanceToPlayer <= magnetRange && distanceToPlayer > minDistanceToStop;
    }


    // �����߼������FixedUpdate����Unity����֡ͬ�������ȶ���
    private void FixedUpdate()
    {
        // ����������Χ����û��Rigidbody2D��ֱ�ӷ���
        if (!isInMagnetRange || rb == null)
            return;

        // 3. ���㡰��Դ����ҡ��ĳ�����������һ����ֻ��������
        Vector2 magnetDirection = (playerTarget.position - transform.position).normalized;

        // 4. ʩ�������������deltaTime��ȷ��֡���޹أ�
        rb.AddForce(magnetDirection * magnetForce * Time.fixedDeltaTime, ForceMode2D.Force);

        // 5. ���������ٶȣ�������Դ�ٶȹ��죩
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


    // �����жϣ��Ƿ�������������������չ��������Դ�Ƿ񼤻�Ƿ����������赲�ȣ�
    private bool CanMagnet()
    {
        // ������ԼӶ������������磺��Դδ��ʰȡ����Ҵ���
        return gameObject.activeSelf;
    }


    // ��ѡ��Gizmos���ӻ�������Χ��������ͼ�п�����ɫԲȦ��������ԣ�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, magnetRange); // ������ΧȦ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistanceToStop); // ֹͣ����Ȧ
    }
}
