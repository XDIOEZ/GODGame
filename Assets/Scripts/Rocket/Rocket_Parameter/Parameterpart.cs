using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterPart : MonoBehaviour
{
    [Header("吸附配置")]
    public float magnetRange = 5f;          // 吸附触发范围
    public float magnetForce = 20f;         // 吸附力强度
    public float maxMagnetSpeed = 3f;       // 吸附最大速度
    public float minDistanceToStop = 0.5f;  // 最小停止距离

    [Header("引用")]
    public Transform playerTarget;          // 玩家的Transform
    private Rigidbody2D rb;                 // 资源自身的Rigidbody2D

    [Header("要检测的目标图层")]
    public LayerMask targetLayer;

    [Header("需要绑定的飞船标签名")]
    [SerializeField]
    private string playerName = "Player";

    private bool isInMagnetRange = false;   // 是否在吸附范围内


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // 初始化Rigidbody2D状态，确保不受重力影响且初始静止
        if (rb != null)
        {
            rb.gravityScale = 0;            // 禁用重力
            rb.velocity = Vector2.zero;     // 初始速度设为0
            rb.angularVelocity = 0;         // 初始角速度设为0
        }

        // 自动找到玩家
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerName);
            if (player != null)
                playerTarget = player.transform;
            else
                Debug.LogError("未找到玩家！请设置正确的标签或手动赋值");
        }
    }

    private void Update()
    {
        if (playerTarget == null || !CanMagnet())
        {
            isInMagnetRange = false;
            return;
        }

        // 计算到玩家的距离并更新吸附状态
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);
        isInMagnetRange = distanceToPlayer <= magnetRange && distanceToPlayer > minDistanceToStop;

        // 不在吸附范围时，确保物体静止
        if (!isInMagnetRange && rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
    }


    private void FixedUpdate()
    {
        if (!isInMagnetRange || rb == null)
            return;

        // 计算吸附方向
        Vector2 magnetDirection = (playerTarget.position - transform.position).normalized;

        // 施加吸附力
        rb.AddForce(magnetDirection * magnetForce * Time.fixedDeltaTime, ForceMode2D.Force);

        // 限制最大速度
        if (rb.velocity.magnitude > maxMagnetSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxMagnetSpeed;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            EventManager.Instance.Emit(new ParameterPartsTouchEvent());
            Destroy(this.gameObject);
        }
    }

    private bool CanMagnet()
    {
        return gameObject.activeSelf;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistanceToStop);
    }
}
