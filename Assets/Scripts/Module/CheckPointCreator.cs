using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckPointCreator : MonoBehaviour
{
    [Header("����")]
    public PlayerController controller; // Inspector ���� PlayerController

    [Header("������ϵͳ Action")]
    public string inputActionName = "R"; // ���� Inspector �޸�

    [Header("UI ��ť (�ƶ���)")]
    public Button button; // ��ť

    [Header("��������")]
    public GameObject owner; // ��������

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
            action.started += ctx => Enable();
            action.canceled += ctx => Disable();
        }
    }

    private void Disable()
    {

    }

    private void Enable()
    {
        Checkpoint.CreateNewCheckpoint(owner.transform.position);
    }

    private void BindButton()
    {
        if (button == null) return;

        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        AddEvent(trigger, EventTriggerType.PointerDown, Enable);
        AddEvent(trigger, EventTriggerType.PointerUp, Disable);
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
}
