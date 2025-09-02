using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundthorn : MonoBehaviour
{
    [Header("��������")]
    public int demage = 1;
    [Tooltip("Ҫ����Ŀ��ͼ�㣨��Player�㣩")]
    public LayerMask targetLayer;        // ͳһʹ��Layer��⣨��������
    [Tooltip("�˺���ȴʱ�䣨�룩")]
    public float damageCooldown = 1f;  // �˺���ȴʱ�䣬Ĭ��Ϊ1��

    // ����״̬�����ɼ�ʱ�����Ƶ���ȴ���
    private bool _isInCooldown;
    // ��ʱ��ID������������ʱ������������ȴ�߼�
    private string _cooldownTimerId;

    void Start()
    {

    }

    void Update()
    {

    }

    // ��ײ��⣺ͨ��Layer�жϽӴ�����������
    private void OnCollisionStay2D(Collision2D other)
    {
        // �����ײ�����Ƿ���Ŀ��ͼ����
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            DealDamage();
        }
    }

    private void OnDisable()
    {

    }
    private void DealDamage()
    {
        Debug.Log(_isInCooldown);
        if (!_isInCooldown)
        {
            EventManager.Instance.Emit(new ParameterShipDurability(Durability: -demage));
            StartCooldown();
        }
    }

    private void StartCooldown()
    {
        _isInCooldown = true;

        // ������ɼ�ʱ���������ظ���ʱ��
        if (!string.IsNullOrEmpty(_cooldownTimerId))
        {
            TimerManager.Instance.RemoveTimer(_cooldownTimerId);
        }
        // �ӳ� 2 ���ִ�лص�
        _cooldownTimerId = TimerManager.Instance.AddTimer(damageCooldown, () =>
        {
            Debug.Log("��ʱ���ص���������ǰʱ�䣺" + Time.time); // �������У�ȷ������ִ��
            _isInCooldown = false;
            _cooldownTimerId = null; // ���ID���������
            // ��������д�����Ŷ�����������������ˢ�� UI ��
        });
    }
}
