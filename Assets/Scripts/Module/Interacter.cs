using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Interacter : MonoBehaviour
{
    [Header("引用")]
    public PlayerController controller; // Inspector 里拖 PlayerController

    [Header("新输入系统 Action")]
    public string inputActionName = "E"; // 可在 Inspector 修改

    [Header("UI 按钮 (移动端)")]
    public Button button; // 按钮

    [Header("所属对象")]
    public GameObject owner; // 所属对象

    public Collider2D InteractCollider; // 碰撞体

    void Start()
    {
        if (owner == null)
            owner = transform.parent.gameObject;

        BindKeyboard();
        BindButton();
    }

    #region 输入绑定
    private void BindKeyboard()
    {
        var action = controller.playerInput.actions[inputActionName];
        if (action != null)
        {
            action.started += ctx => EnableCollider();
            action.canceled += ctx => DisableCollider();
        }
    }

    private void BindButton()
    {
        if (button == null) return;

        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        AddEvent(trigger, EventTriggerType.PointerDown, EnableCollider);
        AddEvent(trigger, EventTriggerType.PointerUp, DisableCollider);
    }

    private void AddEvent(EventTrigger trigger, EventTriggerType type, UnityEngine.Events.UnityAction action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = type
        };
        entry.callback.AddListener((data) => action.Invoke());
        trigger.triggers.Add(entry);
    }
    #endregion

    private void EnableCollider()
    {
        if (InteractCollider != null)
            InteractCollider.enabled = true;
    }

    private void DisableCollider()
    {
        if (InteractCollider != null)
            InteractCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<InteractReciver>(out InteractReciver reciver))
        {
            reciver.Interact(this);
        }
    }
}
