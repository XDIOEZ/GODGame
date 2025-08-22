using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Engine : MonoBehaviour
{
    public enum EngineType { Left, Right }

    [Header("引用")]
    public PlayerController controller; // Inspector 里拖 PlayerController
    public Button engineButton;         // 每个引擎对应一个按钮
    public Rigidbody2D rb;
    public GameObject engineParticles;

    [Header("推进设置")]
    public float thrustForce = 5f;      // 单个引擎推力
    public float torqueForce = 5f;

    [Header("引擎类型")]
    public EngineType engineType;

    [Header("新输入系统 Action")]
    public string inputActionName = "LeftFly"; // 可在 Inspector 修改

    private bool engineActive;

    private void Awake()
    {
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
        // 粒子控制
        if (engineParticles != null)
            engineParticles.SetActive(engineActive);

        if (!engineActive) return;

        // 帧率无关的推力和扭矩
        float delta = Time.deltaTime; // 当前帧时间间隔

        // 推力
        rb.AddForce(transform.up * thrustForce * delta, ForceMode2D.Force);

        // 扭矩
        float torque = (engineType == EngineType.Left ? -torqueForce : torqueForce) * delta;
        rb.AddTorque(torque, ForceMode2D.Force);
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
}
