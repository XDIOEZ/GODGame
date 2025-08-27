using UnityEngine;

public class BackgroundBoxFader : MonoBehaviour
{
    [Header("�ҽӶ���")]
    public SpriteRenderer objectA; // ��������
    public SpriteRenderer objectB; // ���Զ���

    private float minY; // ��ײ���ײ� Y
    private float maxY; // ��ײ������ Y

    private void Awake()
    {
        // ��ȡ��ײ�������±߽�
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Bounds b = col.bounds;
            minY = b.min.y;
            maxY = b.max.y;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ȷ�������
        {
            float playerY = collision.transform.position.y;

            // �����һ������ 0~1
            float t = Mathf.InverseLerp(minY, maxY, playerY);
            t = Mathf.Clamp01(t);

            // objectA Խ����͸����Խ��
            if (objectA != null)
            {
                Color cA = objectA.color;
                cA.a = 1f - t;
                objectA.color = cA;
            }

            // objectB Խ����͸����Խ��
            if (objectB != null)
            {
                Color cB = objectB.color;
                cB.a = t;
                objectB.color = cB;
            }
        }
    }
}
