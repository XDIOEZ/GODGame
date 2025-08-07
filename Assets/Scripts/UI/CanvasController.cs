using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasController : MonoBehaviour
{
    private Dictionary<string, UIBehaviour> controlDic = new();
    public CanvasGroup canvasGroup;
    public bool CanDrag = false;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // 自动收集常见控件
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<InputField>();
    }

    protected virtual void Start()
    {
        // 自动绑定“关闭页面”按钮
        foreach (var pair in controlDic)
        {
            if (pair.Key.Contains("关闭页面") && pair.Value is Button btn)
            {
                btn.onClick.AddListener(() => Close());
            }
        }
    }

    // 自动查找所有 T 类型控件并加入 controlDic
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>(true);
        foreach (var control in controls)
        {
            if (control == null || string.IsNullOrEmpty(control.name)) continue;
            if (!controlDic.ContainsKey(control.name))
                controlDic.Add(control.name, control);
            else
                Debug.LogWarning($"重复的控件名：{control.name}，请检查 UI 结构。");
        }
    }

#if UNITY_EDITOR
    [Button("显示面板")]
#endif
    public void Open()
    {
        SetCanvasGroupState(true);
    }

#if UNITY_EDITOR
    [Button("关闭面板")]
#endif
    public void Close()
    {
        SetCanvasGroupState(false);
    }

    public bool IsOpen()
    {
        return canvasGroup != null &&
               canvasGroup.alpha > 0 &&
               canvasGroup.interactable &&
               canvasGroup.blocksRaycasts;
    }

#if UNITY_EDITOR
    [Button("切换面板显隐")]
#endif
    public void Toggle()
    {
        SetCanvasGroupState(!IsOpen());
    }

    /// <summary>
    /// 封装 CanvasGroup 显示/隐藏切换逻辑
    /// </summary>
    private void SetCanvasGroupState(bool isOpen)
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = isOpen ? 1 : 0;
        canvasGroup.interactable = isOpen;
        canvasGroup.blocksRaycasts = isOpen;
    }

    /// <summary>
    /// 切换所有或指定面板的 CanvasGroup 状态
    /// </summary>
    /// <param name="panelName">为 null 时切换全部</param>
    public void TogglePanel(string panelName = null)
    {
        foreach (var pair in controlDic)
        {
            if (!string.IsNullOrEmpty(panelName) && pair.Key != panelName)
                continue;

            var uiElement = pair.Value;
            if (uiElement == null) continue;

            var group = uiElement.GetComponent<CanvasGroup>();
            if (group == null) continue;

            bool isOpen = group.alpha > 0 && group.interactable && group.blocksRaycasts;
            group.alpha = isOpen ? 0 : 1;
            group.interactable = !isOpen;
            group.blocksRaycasts = !isOpen;
        }
    }
}
