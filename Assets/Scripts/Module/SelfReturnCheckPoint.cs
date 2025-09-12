using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class SelfReturnCheckPoint : MonoBehaviour
{
    [Header("����")]
    public PlayerController controller; // Inspector ���� PlayerController

    [Header("������ϵͳ Action")]
    public string inputActionName = "R"; // ���� Inspector �޸�

    [Header("UI ��ť (�ƶ���)")]
    public Button button; // ��ť

    [Header("��������")]
    public GameObject owner; // ��������

    private InputAction action;
    private EventTrigger trigger;

    void Start()
    {
        if (owner == null)
            owner = transform.parent?.gameObject;

        BindKeyboard();
        BindButton();
    }

    #region �����
    private void BindKeyboard()
    {
        if (controller == null || controller.playerInput == null) return;

        action = controller.playerInput.actions[inputActionName];
        if (action != null)
        {
            action.started += OnActionStarted;
            action.canceled += OnActionCanceled;
        }
    }

    private void OnActionStarted(InputAction.CallbackContext ctx) => Enable();
    private void OnActionCanceled(InputAction.CallbackContext ctx) => Disable();

    private void Disable()
    {
        // TODO: �������Ҫ�ָ�״̬��д�߼�
    }

    private void Enable()
    {
        Checkpoint.BackToCurrentActiveCheckpoint(owner);
    }

    private void BindButton()
    {
        if (button == null) return;

        trigger = button.gameObject.GetComponent<EventTrigger>();
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

    private void OnDestroy()
    {
        // ��������¼�
        if (action != null)
        {
            action.started -= OnActionStarted;
            action.canceled -= OnActionCanceled;
            action = null;
        }

        // ���� UI EventTrigger
        if (trigger != null && trigger.triggers != null)
        {
            trigger.triggers.Clear();
            trigger = null;
        }

        button = null;
        controller = null;
        owner = null;
    }
}
