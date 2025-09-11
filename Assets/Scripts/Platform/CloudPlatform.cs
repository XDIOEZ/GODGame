using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPlatform : PlatformBase
{
    [Header("��������")]
    public float disappearDuration = 3f; // ��ʧ����Ӵ�ʱ��
    public float fadeSpeed = 2f;         // �����ٶ�
    [Tooltip("Ҫ����Ŀ��ͼ�㣨��Player�㣩")]
    public LayerMask targetLayer;        // ͳһʹ��Layer���

    [Header("�ָ�����")]
    [Tooltip("�Ƿ������뿪��ָ�ԭ״����ʧǰ��")]
    public bool shouldRecover = true;    // ��ʧǰ�Ƿ�ָ�

    [Tooltip("�Ƿ�������ȫ��ʧ������")]
    public bool canRecover = true;       // ��ȫ��ʧ���Ƿ���������

    [Header("�黯��������")]
    public bool canFall = true;       // �Ƿ������м����
    [Tooltip("���ֵΪ1����СֵΪ0")]
    public float canFallTimeFactor = 0.5f;
    private float FallTimeAdjust;
    
    [Header(("��������ʱ��"))]
    public float recoverDuration = 5f;   // ����ʧ����ʼ�����ļ��ʱ��

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;

    // ״̬����
    private bool isContacting = false;   // �Ƿ�������Ӵ�
    private float contactTimer = 0f;     // �Ӵ���ʱ
    private float targetAlpha = 1f;      // Ŀ��͸����
    private bool isDisappeared = false;  // �Ƿ�����ʧ
    private float recoverTimer = 0f;     // ������ʱ��

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        SetAlpha(1f); // ��ʼ��ȫ��ʾ
    }

    void Update()
    {
        if (isDisappeared)
        {
            // �������������������������ʱ
            if (canRecover)
            {
                recoverTimer += Time.deltaTime;
                if (recoverTimer >= recoverDuration)
                {
                    // �����߼�
                    Recover();
                }
            }
        }
        else if (isContacting)
        {
            // �Ӵ�ʱ����ʱ������ʧ
            contactTimer += Time.deltaTime;
            targetAlpha = 1 - (contactTimer / disappearDuration);
            FallTimeAdjust = canFallTimeFactor * disappearDuration;
            if (canFall)
            {
                if (contactTimer >= FallTimeAdjust)
                {
                    isDisappeared = true;
                    platformCollider.enabled = false; // ������ײ
                }
            }
            if (contactTimer >= disappearDuration)
            {
                    isDisappeared = true;
                    platformCollider.enabled = false; // ������ײ
            }
        }
        else
        {
            // �뿪ʱ�����ݿ��ؾ����Ƿ�ָ�����ʧǰ��
            if (shouldRecover)
            {
                // ����ָ����𽥱�ز�͸��
                targetAlpha += Time.deltaTime * fadeSpeed;
                targetAlpha = Mathf.Clamp01(targetAlpha);

                // ��ȫ�ָ������ü�ʱ
                if (targetAlpha >= 1f)
                {
                    contactTimer = 0f;
                }
            }
            // ������ָ������ֵ�ǰ͸���Ⱥͼ�ʱ
        }



        // Ӧ��͸���Ƚ���
        float currentAlpha = Mathf.MoveTowards(
            spriteRenderer.color.a,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );
        SetAlpha(currentAlpha);
    }

    // ��������������״̬
    private void Recover()
    {
        isDisappeared = false;
        platformCollider.enabled = true;
        targetAlpha = 1f;
        contactTimer = 0f;
        recoverTimer = 0f;
    }

    // ��ײ��⣺ͨ��Layer�жϽӴ�
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

    // ����͸����
    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    // �ⲿ���ƽӿڣ���̬�޸��Ƿ�����ָ�
    public void SetShouldRecover(bool value)
    {
        shouldRecover = value;
    }

    // �ⲿ���ƽӿڣ���̬�޸��Ƿ���������
    public void SetCanRecover(bool value)
    {
        canRecover = value;
    }
}
