using UnityEngine;

public class FuelUp :MonoBehaviour
{
    [Header("ȼ������")]
    public float AddmaxFuel = 0.5f;
    public float followSpeed = 5f;
    public float detectionRadius = 3f;

    [Header("��������")]
    public GameObject playerFuel;
    public LayerMask playerLayer = 1; // Ĭ�ϲ�

    private Transform playerTransform;
    private bool isFollowing = false;
    private Rigidbody2D rb;
    private Collider2D itemCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        itemCollider = GetComponent<Collider2D>();

        // �Զ�������Ҳ㼶
        playerLayer = LayerMask.GetMask("Player");

        // ���û��ָ��playerFuel�������Զ�����
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
        // ��ⷶΧ�ڵ����
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

                // ������ײ���Ա����������
                if (itemCollider != null)
                    itemCollider.enabled = false;

                // �����2D�������ø�������
                if (rb != null)
                    rb.isKinematic = true;

                Debug.Log("��ʼ�������");
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

        // ƽ���ƶ������λ��
        float step = followSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

        // ����Ƿ񵽴����λ��
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
                Debug.Log($"����ȼ������: {parameterFuel.maxFuel}, ��ǰȼ��: {parameterFuel.fuel}");
            }
        }
    }

    // ���ӻ���ⷶΧ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isFollowing ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // �Ƴ�ԭ������ײ��⣬���ô������
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