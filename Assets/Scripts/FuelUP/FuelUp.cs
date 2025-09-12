using UnityEngine;

public class FuelUp : MonoBehaviour
{
    [Header("燃料设置")]
    public float AddmaxFuel = 0.5f;
    public float followSpeed = 5f;
    public float detectionRadius = 3f;

    [Header("层级设置")]
    public LayerMask playerLayer = 1;

    private Transform playerTransform;
    private bool isFollowing = false;
    private Rigidbody2D rb;
    private Collider2D itemCollider;
    private ParameterFuel playerParameterFuel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        itemCollider = GetComponent<Collider2D>();

        // 自动设置玩家层级
        playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        if (!isFollowing)
        {
            DetectPlayer();
        }
        else
        {
            FollowPlayer();
        }
    }

    void DetectPlayer()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);

        foreach (Collider2D player in players)
        {
            if (player.CompareTag("Player"))
            {
                // 获取玩家的燃料组件
                playerParameterFuel = player.GetComponentInChildren<ParameterFuel>();
                if (playerParameterFuel != null)
                {
                    playerTransform = player.transform;
                    isFollowing = true;

                    // 禁用碰撞器以避免物理干扰
                    if (itemCollider != null)
                        itemCollider.enabled = false;

                    // 如果是2D物理，设置刚体类型
                    if (rb != null)
                        rb.isKinematic = true;

                    Debug.Log("开始跟随玩家");
                    break;
                }
            }
        }
    }

    void FollowPlayer()
    {
        if (playerTransform == null || playerParameterFuel == null)
        {
            isFollowing = false;
            return;
        }

        // 平滑移动到玩家位置
        float step = followSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

        // 检查是否到达玩家位置
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance < 0.1f)
        {
            ApplyFuelEffect();
            Destroy(gameObject);
        }
    }

    void ApplyFuelEffect()
    {
        if (playerParameterFuel != null)
        {
            playerParameterFuel.maxFuel += AddmaxFuel;
            playerParameterFuel.fuel = Mathf.Min(playerParameterFuel.fuel + AddmaxFuel, playerParameterFuel.maxFuel);
            Debug.Log($"增加燃料上限: {playerParameterFuel.maxFuel}, 当前燃料: {playerParameterFuel.fuel}");
        }
    }

    // 直接碰撞检测（作为备用方案）
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFollowing)
        {
            // 获取玩家的燃料组件
            playerParameterFuel = collision.gameObject.GetComponentInChildren<ParameterFuel>();
            if (playerParameterFuel != null)
            {
                ApplyFuelEffect();
                Destroy(gameObject);
            }
        }
    }

    // 触发检测（用于跟随）
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFollowing)
        {
            // 获取玩家的燃料组件
            playerParameterFuel = other.GetComponentInChildren<ParameterFuel>();
            if (playerParameterFuel != null)
            {
                playerTransform = other.transform;
                isFollowing = true;

                if (itemCollider != null)
                    itemCollider.enabled = false;

                if (rb != null)
                    rb.isKinematic = true;
            }
        }
    }

    // 可视化检测范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isFollowing ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}