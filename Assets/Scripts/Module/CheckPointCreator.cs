using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckPointCreator : MonoBehaviour
{
    [Header("引用")]
    public PlayerController controller; // Inspector 里拖 PlayerController

    [Header("新输入系统 Action")]
    public string inputActionName = "R"; // 可在 Inspector 修改

    [Header("UI 按钮 (移动端)")]
    public Button button; // 按钮

    [Header("所属对象")]
    public GameObject owner; // 所属对象

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
