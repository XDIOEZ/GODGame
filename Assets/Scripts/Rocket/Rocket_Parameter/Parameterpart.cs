using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterPart : MonoBehaviour
{
    [Header("吸附配置")]
    public float magnetRange = 5f;          // 吸附触发范围（玩家进入这个范围后开始吸附）
    public float magnetForce = 20f;         // 吸附力强度（越大吸得越快，建议5-30调试）
    public float maxMagnetSpeed = 3f;       // 吸附最大速度（避免资源飞太快，建议2-5）
    public float minDistanceToStop = 0.5f;  // 最小停止距离（离玩家小于这个距离时停止吸附，防抖动）

    [Header("引用")]
    public Transform playerTarget;          // 玩家的Transform（可在Inspector拖，或自动找）
    private Rigidbody2D rb;                 // 资源自身的Rigidbody2D（用于物理吸附）

    [Header("要检测的目标图层（如Player层）")]
    public LayerMask targetLayer;        // 统一使用Layer检测（已修正）

    [Header("需要绑定的飞船标签（tag）名")]
    [SerializeField]
    private string playerName;

    private bool isInMagnetRange = false;   // 是否在吸附范围内


    private void Awake()
    {
        // 自动获取资源的Rigidbody2D（确保资源有这个组件，否则用插值移动）
        rb = GetComponent<Rigidbody2D>();

        // 自动找到玩家（如果没手动拖，根据标签找，玩家标签设为"Player"）
        if (playerTarget == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerName);
            if (player != null)
                playerTarget = player.transform;
            else
                Debug.LogError("未找到玩家！请给玩家设置标签'Player'，或手动赋值playerTarget");
        }
    }
    private void Update()
    {
        // 先判断玩家是否存在，以及资源是否还需要吸附
        if (playerTarget == null || !CanMagnet())
        {
            isInMagnetRange = false;
            return;
        }

        // 1. 计算资源到玩家的距离
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        // 2. 判断是否进入吸附范围
        isInMagnetRange = distanceToPlayer <= magnetRange && distanceToPlayer > minDistanceToStop;
    }


    // 物理逻辑建议放FixedUpdate（与Unity物理帧同步，更稳定）
    private void FixedUpdate()
    {
        // 不在吸附范围，或没有Rigidbody2D，直接返回
        if (!isInMagnetRange || rb == null)
            return;

        // 3. 计算“资源→玩家”的朝向向量（归一化，只保留方向）
        Vector2 magnetDirection = (playerTarget.position - transform.position).normalized;

        // 4. 施加吸附力（结合deltaTime，确保帧率无关）
        rb.AddForce(magnetDirection * magnetForce * Time.fixedDeltaTime, ForceMode2D.Force);

        // 5. 限制吸附速度（避免资源速度过快）
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


    // 辅助判断：是否满足吸附条件（可扩展，比如资源是否激活、是否被其他物体阻挡等）
    private bool CanMagnet()
    {
        // 这里可以加额外条件，比如：资源未被拾取、玩家存活等
        return gameObject.activeSelf;
    }


    // 可选：Gizmos可视化吸附范围（场景视图中看到红色圆圈，方便调试）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, magnetRange); // 吸附范围圈
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDistanceToStop); // 停止距离圈
    }
}
