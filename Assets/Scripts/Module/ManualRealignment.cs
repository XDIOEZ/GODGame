using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualRealignment : MonoBehaviour
{
    [Header("UI����")]
    public Button realignmentButton;

    [Header("����")]
    public Transform parentTransform;  // ��Ҫ�����ĸ�����Transform

    [Header("����")]
    public float cooldownTime = 1.0f;  // ��ȴʱ��

    // ˽�б���
    private Quaternion initialRotation; // ��ʼ��ת
    private float lastUseTime = -Mathf.Infinity; // �ϴ�ʹ��ʱ��

    void Start()
    {
        // ��¼��ʼ��ת
        if (parentTransform != null)
        {
            initialRotation = parentTransform.rotation;
        }

        // Ϊ��ť��ӵ���¼�
        if (realignmentButton != null)
        {
            realignmentButton.onClick.AddListener(Realign);
        }
    }

    void Update()
    {
        // ����Ƿ�����ȴ��
        bool isOnCooldown = Time.time < lastUseTime + cooldownTime;

        // ���°�ť״̬
        if (realignmentButton != null)
        {
            realignmentButton.interactable = !isOnCooldown;
        }
    }

    // ������������������ת��
    private void Realign()
    {
        // ����Ƿ�������
        if (parentTransform == null)
            return;

        // ����Ƿ�����ȴ��
        if (Time.time < lastUseTime + cooldownTime)
            return;

        // ˲�������ת
        parentTransform.rotation = initialRotation;

        // ��¼����ʹ��ʱ��
        lastUseTime = Time.time;
    }
}
