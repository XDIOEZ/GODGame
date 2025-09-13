using UnityEngine;

public class FuelUp :MonoBehaviour
{
    [Header("燃料设置")]
    public float AddmaxFuel = 0.5f;
    public float followSpeed = 5f;
    public float detectionRadius = 3f;

    [Header("引用设置")]
    public GameObject playerFuel;
    public LayerMask playerLayer = 1; // 默认层

    private Transform playerTransform;
    private bool isFollowing = false;
    private Rigidbody2D rb;
    private Collider2D itemCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        itemCollider = GetComponent<Collider2D>();

        // 自动设置玩家层级
        playerLayer = LayerMask.GetMask("Player");

        // 如果没有指定playerFuel，尝试自动查找
        if (playerFuel == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerFuel = player;
            }
        }
    }

    void Update()
    {
        // 检测范围内的玩家
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

    void FollowPlayer()
    {
        if (playerTransform == null)
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
        if (playerFuel != null)
        {
            ParameterFuel parameterFuel = playerFuel.GetComponent<ParameterFuel>();
            if (parameterFuel != null)
            {
                parameterFuel.maxFuel += AddmaxFuel;
                parameterFuel.fuel = Mathf.Min(parameterFuel.fuel + AddmaxFuel, parameterFuel.maxFuel);
                Debug.Log($"增加燃料上限: {parameterFuel.maxFuel}, 当前燃料: {parameterFuel.fuel}");
            }
        }
    }

    // 可视化检测范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isFollowing ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // 移除原来的碰撞检测，改用触发检测
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFollowing)
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