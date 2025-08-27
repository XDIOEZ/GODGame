using UnityEngine;

public class BackgroundBoxFader : MonoBehaviour
{
    [Header("挂接对象")]
    public SpriteRenderer objectA; // 渐隐对象
    public SpriteRenderer objectB; // 渐显对象

    private float minY; // 碰撞器底部 Y
    private float maxY; // 碰撞器顶部 Y

    private void Awake()
    {
        // 获取碰撞器的上下边界
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
        if (collision.CompareTag("Player")) // 确保是玩家
        {
            float playerY = collision.transform.position.y;

            // 计算归一化进度 0~1
            float t = Mathf.InverseLerp(minY, maxY, playerY);
            t = Mathf.Clamp01(t);

            // objectA 越往上透明度越低
            if (objectA != null)
            {
                Color cA = objectA.color;
                cA.a = 1f - t;
                objectA.color = cA;
            }

            // objectB 越往上透明度越高
            if (objectB != null)
            {
                Color cB = objectB.color;
                cB.a = t;
                objectB.color = cB;
            }
        }
    }
}
