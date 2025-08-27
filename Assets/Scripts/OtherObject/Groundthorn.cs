using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundthorn : MonoBehaviour
{
    [Header("��������")]
    public float demage = 1f;
    [Tooltip("Ҫ����Ŀ��ͼ�㣨��Player�㣩")]
    public LayerMask targetLayer;        // ͳһʹ��Layer��⣨��������
    [Tooltip("�˺���ȴʱ�䣨�룩")]
    public float damageCooldown = 1f;  // �˺���ȴʱ�䣬Ĭ��Ϊ1��

    // ״̬����
    private bool isContacting = false;
    private float lastDamageTime = -Mathf.Infinity;  // �ϴ�����˺���ʱ��

    void Start()
    {

    }

    void Update()
    {
        // �����Ŀ��Ӵ��Ҳ�����ȴ�У�����˺�
        if (isContacting &&Time.time - lastDamageTime >= damageCooldown)
        {
            DealDamage();
            lastDamageTime = Time.time;  // ��¼�����˺�ʱ��
        }
    }

    // ��ײ��⣺ͨ��Layer�жϽӴ�����������
    private void OnCollisionEnter2D(Collision2D other)
    {
        // �����ײ�����Ƿ���Ŀ��ͼ����
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            isContacting = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // ����뿪�����Ƿ���Ŀ��ͼ����
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            isContacting = false;
        }
    }


    private void DealDamage()
    {
        EventManager.Instance.Emit(new ParameterShipDurability(Durability: -1));
    }
}
