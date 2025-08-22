using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : PlatformBase
{
    [Header("�ƶ�����")]
    public Transform pointA;       // A�����ã� Inspector ��ק��ֵ ��
    public Transform pointB;       // B�����ã� Inspector ��ק��ֵ ��
    public float moveSpeed = 2f;   // ƽ̨�ƶ��ٶ�
    public bool isLooping = true;  // �Ƿ�ѭ������

    [Header("�־û�����")]
    public bool usePersistence = true; // �Ƿ����ó־û���¼
    private Vector3 initialPosition;   // ��ʼλ�ã����ڻָ���
    private float savedProgress = 0f;  // �־û�������ƶ�����

    // ����ʱ״̬
    private float moveProgress = 0f;   // �ƶ����ȣ�0~1 ��һ����
    private int moveDirection = 1;     // �ƶ�����1=����-1=����

    void Start()
    {
        // ��ʼ���־û�����
        if (usePersistence)
        {
            // �� PlayerPrefs ��ȡ�ϴ�����ʱ����Ľ���
            savedProgress = PlayerPrefs.GetFloat("MovablePlatformProgress", 0f);
            moveProgress = savedProgress;
        }

        // ��¼��ʼλ�ã��������ã�
        initialPosition = transform.position;
        UpdatePlatformPosition(); // Ӧ�ó�ʼ����
    }

    void Update()
    {
        // �����ƶ�����
        moveProgress += moveDirection * moveSpeed * Time.deltaTime;
        moveProgress = Mathf.Clamp(moveProgress, 0f, 1f);

        // ����˵�ʱת��
        if (isLooping)
        {
            if (moveProgress >= 1f || moveProgress <= 0f)
            {
                moveDirection *= -1; // ����
            }
        }
        else
        {
            // ��ѭ��ģʽ������˵��ֹͣ
            moveProgress = Mathf.Clamp(moveProgress, 0f, 1f);
            if (moveProgress >= 1f || moveProgress <= 0f)
            {
                moveDirection = 0; // ֹͣ�ƶ�
            }
        }

        // ����ƽ̨λ��
        UpdatePlatformPosition();
    }

    /// <summary>
    /// ���ݹ�һ�����ȸ���ƽ̨λ��
    /// </summary>
    private void UpdatePlatformPosition()
    {
        // ����A��B�Ĳ�ֵλ��
        Vector3 targetPos = Vector3.Lerp(pointA.position, pointB.position, moveProgress);
        transform.position = targetPos;
    }

    /// <summary>
    /// �־û���¼������ʱ���浱ǰ����
    /// </summary>
    private void OnDestroy()
    {
        if (usePersistence)
        {
            PlayerPrefs.SetFloat("MovablePlatformProgress", moveProgress);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// �ⲿ�ӿڣ�����ƽ̨����ʼ״̬
    /// </summary>
    public void ResetPlatform()
    {
        transform.position = initialPosition;
        moveProgress = 0f;
        moveDirection = 1;

        // ����־û����ݣ���ѡ��
        if (usePersistence)
        {
            PlayerPrefs.DeleteKey("MovablePlatformProgress");
            PlayerPrefs.Save();
        }
    }

    // �����ã���Scene��ͼ��ʾA��B��
    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.1f);
            Gizmos.DrawSphere(pointB.position, 0.1f);
        }
    }
}
