using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DualEngineController : MonoBehaviour
{
    [Header("燃料参数")]
    public ParameterFuel fuelData;   // 使用 ParameterFuel 脚本

    [Header("左引擎推力与旋转参数")]
    public float leftThrustPower = 5f;
    public float leftTorquePower = 5f;

    [Header("右引擎推力与旋转参数")]
    public float rightThrustPower = 5f;
    public float rightTorquePower = 5f;

    [Header("引用 (自动获取)")]
    public PlayerController controller;
    public Rigidbody2D rb;

    [Header("UI按钮")]
    public Button leftEngineButton;
    public Button rightEngineButton;

    [Header("粒子效果")]
    public GameObject leftEngineParticles;
    public GameObject rightEngineParticles;

    private bool leftActive;
    private bool rightActive;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponentInParent<Rigidbody2D>();

        if (controller == null)
            controller = GetComponentInChildren<PlayerController>();

        AddButtonEvents(leftEngineButton, () => leftActive = true, () => leftActive = false);
        AddButtonEvents(rightEngineButton, () => rightActive = true, () => rightActive = false);

        if (controller != null && controller.playerInput != null)
        {
            var leftAction = controller.playerInput.actions["A"];
            var rightAction = controller.playerInput.actions["D"];

            if (leftAction != null)
            {
                leftAction.started += ctx => leftActive = true;
                leftAction.canceled += ctx => leftActive = false;
            }
            if (rightAction != null)
            {
                rightAction.started += ctx => rightActive = true;
                rightAction.canceled += ctx => rightActive = false;
            }
        }
    }

private void Update()
{
    // 粒子效果控制保留在 Update 中
    if (leftEngineParticles != null) leftEngineParticles.SetActive(leftActive && fuelData.fuel > 0);
    if (rightEngineParticles != null) rightEngineParticles.SetActive(rightActive && fuelData.fuel > 0);

    // 燃料消耗也保留在 Update 中（因为燃料消耗与视觉反馈更相关）
    if (fuelData != null && fuelData.fuel > 0 && (leftActive || rightActive))
    {
        fuelData.fuel -= Time.deltaTime * fuelData.FuelCoefficient;
        if (fuelData.fuel < 0) fuelData.fuel = 0;
    }
}

private void FixedUpdate()
{
    // 物理计算移到 FixedUpdate 中，避免帧率影响
    if (fuelData == null || fuelData.fuel <= 0 || rb == null) return;

    Vector2 velocity = rb.velocity;
    float angularVel = rb.angularVelocity;

    if (leftActive)
    {
        velocity += (Vector2)transform.up * leftThrustPower * Time.fixedDeltaTime;
        angularVel -= leftTorquePower;
    }

    if (rightActive)
    {
        velocity += (Vector2)transform.up * rightThrustPower * Time.fixedDeltaTime;
        angularVel += rightTorquePower;
    }

    rb.velocity = velocity;
    rb.angularVelocity = angularVel;
}

    private void AddButtonEvents(Button button, System.Action onDown, System.Action onUp)
    {
        if (button == null) return;

        var trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        var entryDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entryDown.callback.AddListener((_) => onDown());
        trigger.triggers.Add(entryDown);

        var entryUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entryUp.callback.AddListener((_) => onUp());
        trigger.triggers.Add(entryUp);

        var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((_) => onUp());
        trigger.triggers.Add(entryExit);
    }

    // 🔹 绘制最大可飞行范围
    private void OnDrawGizmosSelected()
    {
        if (fuelData == null) return;

        float avgThrust = (leftThrustPower + rightThrustPower) / 2f;
        float fuelTime = fuelData.maxFuel / fuelData.FuelCoefficient;

        // 估算最大位移（平均速度公式）
        float maxRange = (avgThrust * fuelTime) / 2f;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxRange);
    }
}
