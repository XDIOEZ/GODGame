using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeBack : MonoBehaviour
{
    // ������ײʱ���� ��ȡRB2D��� ��RB2D�ҽӵ�GameObject ���͵� ��¼��
    void OnCollisionEnter2D(Collision2D collision)
    {
        // �����ײ�����Ƿ�ΪPlayerͼ��
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // ��ȡRB2D���
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            // ����Ƿ����RB2D���
            if (rb != null)
            {
                // ��RB2D�ҽӵ�GameObject���͵���¼��
                TransportToRecordPoint(collision.gameObject);
            }
        }
    }

    // �������Ĵ��ͽӿڷ���
    private void TransportToRecordPoint(GameObject obj)
    {
        Checkpoint.BackToCurrentActiveCheckpoint(obj);
    }
}