using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Engine : MonoBehaviour
{
    public enum EngineType { Left, Right }

    [Header("重力系数参数")]
    public ParameterFallingspeed fallingspeed;

    [Header("推力参数")]
    public ParameterEngine Thrust;

    [Header("推力参数key名")]
    public string ThrustKey;
    [Header("当前取key推力参数")]
    public ThrustData CurrentTrustData ;

    [Header("引用")]
    public PlayerController controller; // Inspector 里拖 PlayerController
    public Button engineButton;         // 每个引擎对应一个按钮
    public Rigidbody2D rb;
    public GameObject engineParticles;

    [Header("当前剩余燃料")]
    public ParameterFuel Fuel;

    [Header("当前所处重力")]
    private float FallingSpeed=1;

    [Header("推进设置")]
    public float thrustForce = 5f;      // 单个引擎推力
    public float torqueForce = 5f;

    //基础重力值
    private float gravityForce = 200;

    public float stabilizationFactor = 0f; // 回正系数（越大回正越快，建议从1-5调试）
    public float maxStabilizationTorque; // 最大回正扭矩（避免过度修正）

    [Header("引擎类型")]
    public EngineType engineType;

    [Header("新输入系统 Action")]
    public string inputActionName = "LeftFly"; // 可在 Inspector 修改

    private bool engineActive;

    private void Awake()
    {
        FallingSpeed = fallingspeed.Fallingspeed;
        stabilizationFactor = fallingspeed.ReturnCoefficient;
        maxStabilizationTorque = fallingspeed.ReturnCoefficient;

        if (rb == null && controller != null)
            rb = controller.rb;

        // 根据本地坐标自动判断左右引擎
        float localX = transform.localPosition.x;
        engineType = localX >= 0 ? EngineType.Right : EngineType.Left;

        // UI按钮事件
        AddButtonEvents(engineButton, () => engineActive = true, () => engineActive = false);

        // 新输入系统监听
        if (controller != null && !string.IsNullOrEmpty(inputActionName))
        {
            var action = controller.playerInput.actions[inputActionName];
            if (action != null)
            {
                action.started += ctx => engineActive = true;
                action.canceled += ctx => engineActive = false;
            }
            else
            {
                Debug.LogWarning($"InputAction '{inputActionName}' not found on PlayerInput.");
            }
        }
    }

    private void Update()
    {
        SimulationGravity();
        // 粒子控制
        if (engineParticles != null)
        {
            engineParticles.SetActive(engineActive);
            if (Fuel.fuel <= 0)
            {
                engineParticles.SetActive(false);
            }
        }

        if (!engineActive) return;

        //燃料耗尽
        if (Fuel.fuel<=0)
            return;

        //测试用，后期必删↓
        //if (Fuel.fuel < 100)
        //{
        //    EventManager.Instance.Emit(new ParameterFallingspeed(FallSpeed: 3));
        //    FallingSpeed = fallingspeed.Fallingspeed;
        //}
        //测试用，后期必删↑

        // 帧率无关的推力和扭矩
        float delta = Time.deltaTime; // 当前帧时间间隔
        Fuel.fuel -= (Time.deltaTime *Fuel.FuelCoefficient);

        // 推力
        rb.AddForce(transform.up * (thrustForce) * delta, ForceMode2D.Force);

        // 扭矩
        float torque = (engineType == EngineType.Left ? -torqueForce : torqueForce) * delta;
        rb.AddTorque(torque, ForceMode2D.Force);
    }
    private void FixedUpdate()
    {
        StabilizeDirection();
    }

    private void OnValidate()
    {
        // 校验依赖是否存在
        if (Thrust == null)
        {
            CurrentTrustData = default; // 清空当前数据
            return;
        }

        // 当key为空时清空当前数据
        if (string.IsNullOrEmpty(ThrustKey))
        {
            CurrentTrustData = default;
            return;
        }

        // 从字典中查询key对应的ThrustData并赋值
        if (Thrust.TryGetThrustData(ThrustKey, out ThrustData data))
        {
            CurrentTrustData = data;
            thrustForce = CurrentTrustData.ThrustPower;
            torqueForce = CurrentTrustData.TorqueForcePower;
            engineType = CurrentTrustData.EngineType;
            FallingSpeed = fallingspeed.Fallingspeed;
        }
        else
        {
            // 若key不存在，清空数据并提示
            CurrentTrustData = default;
            Debug.LogWarning($"推力参数中不存在key：{ThrustKey}");
        }
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


    //模拟重力作用方法(下坠速度——重力系数专用)
    private void SimulationGravity()
    {
        FallingSpeed = fallingspeed.Fallingspeed;
        float delta = Time.deltaTime; // 当前帧时间间隔
        rb.AddForce(Vector2.down * (gravityForce) * delta*FallingSpeed, ForceMode2D.Force);
    }

    // 方向稳定逻辑：让火箭慢慢回正到向上方向
    private void StabilizeDirection()
    {
        // 1. 获取当前火箭的朝向（局部上方向）
        Vector2 currentUp = transform.up;

        // 2. 计算当前朝向与世界向上方向（Vector2.up）的夹角（角度制，-180~180）
        float angleToUp = Vector2.SignedAngle(-currentUp, Vector2.up);

        // 3. 根据夹角计算修正扭矩：角度差越大，修正力越强（乘以回正系数）
        stabilizationFactor = fallingspeed.ReturnCoefficient;
        float correctionTorque = angleToUp * stabilizationFactor*0.01f;

        // 4. 限制最大扭矩，避免回正过猛（可选，但建议加）
        correctionTorque = Mathf.Clamp(correctionTorque, -maxStabilizationTorque, maxStabilizationTorque);

        // 5. 施加修正扭矩（注意：扭矩方向与角度差相反，才能回正）
        rb.AddTorque(-correctionTorque * Time.fixedDeltaTime, ForceMode2D.Force);
    }
}
