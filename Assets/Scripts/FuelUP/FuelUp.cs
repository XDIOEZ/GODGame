using UnityEngine;

public class FuelUp : MonoBehaviour
{
    [Header("ȼ������")]
    public float AddmaxFuel = 0.5f;
    public float followSpeed = 5f;
    public float detectionRadius = 3f;

    [Header("�㼶����")]
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

        // �Զ�������Ҳ㼶
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
                // ��ȡ��ҵ�ȼ�����
                playerParameterFuel = player.GetComponentInChildren<ParameterFuel>();
                if (playerParameterFuel != null)
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
    }

    void FollowPlayer()
    {
        if (playerTransform == null || playerParameterFuel == null)
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
        if (playerParameterFuel != null)
        {
            playerParameterFuel.maxFuel += AddmaxFuel;
            playerParameterFuel.fuel = Mathf.Min(playerParameterFuel.fuel + AddmaxFuel, playerParameterFuel.maxFuel);
            Debug.Log($"����ȼ������: {playerParameterFuel.maxFuel}, ��ǰȼ��: {playerParameterFuel.fuel}");
        }
    }

    // ֱ����ײ��⣨��Ϊ���÷�����
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFollowing)
        {
            // ��ȡ��ҵ�ȼ�����
            playerParameterFuel = collision.gameObject.GetComponentInChildren<ParameterFuel>();
            if (playerParameterFuel != null)
            {
                ApplyFuelEffect();
                Destroy(gameObject);
            }
        }
    }

    // ������⣨���ڸ��棩
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFollowing)
        {
            // ��ȡ��ҵ�ȼ�����
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

    // ���ӻ���ⷶΧ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isFollowing ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}