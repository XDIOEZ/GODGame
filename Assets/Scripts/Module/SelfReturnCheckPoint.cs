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
    [Header("引用")]
    public PlayerController controller; // Inspector 里拖 PlayerController

    [Header("新输入系统 Action")]
    public string inputActionName = "R"; // 可在 Inspector 修改

    [Header("UI 按钮 (移动端)")]
    public Button button; // 按钮

    [Header("所属对象")]
    public GameObject owner; // 所属对象

    private InputAction action;
    private EventTrigger trigger;

    void Start()
    {
        if (owner == null)
            owner = transform.parent?.gameObject;

        BindKeyboard();
        BindButton();
    }

    #region 输入绑定
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
        // TODO: 如果有需要恢复状态可写逻辑
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
        // 解绑输入事件
        if (action != null)
        {
            action.started -= OnActionStarted;
            action.canceled -= OnActionCanceled;
            action = null;
        }

        // 清理 UI EventTrigger
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
