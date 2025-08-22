using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Interacter : MonoBehaviour
{
    [Header("����")]
    public PlayerController controller; // Inspector ���� PlayerController

    [Header("������ϵͳ Action")]
    public string inputActionName = "E"; // ���� Inspector �޸�

    [Header("UI ��ť (�ƶ���)")]
    public Button button; // ��ť

    [Header("��������")]
    public GameObject owner; // ��������

    public Collider2D InteractCollider; // ��ײ��

    void Start()
    {
        if (owner == null)
            owner = transform.parent.gameObject;

        BindKeyboard();
        BindButton();
    }

    #region �����
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
