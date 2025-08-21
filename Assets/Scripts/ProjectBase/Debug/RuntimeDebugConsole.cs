using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

/// <summary>
/// Mobile-only IMGUI Debug Console
/// - 浮动按钮 + 可展开面板
/// - EnhancedTouch（New Input System）
/// - 支持调整字体大小 / 行间距
/// </summary>
public class RuntimeDebugConsole : MonoBehaviour
{
    [Header("Appearance")]
    public float floatSize = 72f;
    public float expandedWidthRatio = 0.82f;
    public float expandedHeightRatio = 0.48f;
    public float titleBarHeight = 28f;

    [Header("Behavior")]
    public bool startExpanded = false;

    [Header("Logs")]
    public float lineSpacing = 2f;
    public int fontSize = 28;

    private bool isExpanded;
    private Rect floatRect;
    private Rect expandedRect;
    private Vector2 scrollPos = Vector2.zero;
    private readonly List<string> logs = new List<string>();
    private const int maxLogs = 500;

    // touch/drag
    private bool dragging = false;
    private int draggingTouchId = -1;
    private Vector2 dragStartTouchGui;
    private Vector2 dragStartWindowPos;

    // collected release points in IMGUI coords (origin top-left)
    private readonly List<Vector2> releasePoints = new List<Vector2>();

    private readonly int floatWindowId = 0xFA1B;
    private readonly int expandedWindowId = 0xFA1C;

    // settings
    private bool showSettings = false;

    private void Awake()
    {
        isExpanded = startExpanded;
        InitRects();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        EnhancedTouchSupport.Disable();
    }

    private void InitRects()
    {
        float margin = 10f;
        float x = Screen.width - floatSize - margin;
        float y = Screen.height - floatSize - margin;
        Rect safe = Screen.safeArea;
        x = Mathf.Clamp(x, safe.xMin, safe.xMax - floatSize);
        y = Mathf.Clamp(y, safe.yMin, safe.yMax - floatSize);
        floatRect = new Rect(x, y, floatSize, floatSize);

        float w = Mathf.Max(220, Screen.width * expandedWidthRatio);
        float h = Mathf.Max(160, Screen.height * expandedHeightRatio);
        float ex = (Screen.width - w) * 0.5f;
        float ey = (Screen.height - h) * 0.5f;
        ex = Mathf.Clamp(ex, safe.xMin, safe.xMax - w);
        ey = Mathf.Clamp(ey, safe.yMin, safe.yMax - h);
        expandedRect = new Rect(ex, ey, w, h);
    }

    private void Update()
    {
        // collect touches via EnhancedTouch
        var active = Touch.activeTouches;
        for (int i = 0; i < active.Count; ++i)
        {
            var t = active[i];
            Vector2 screenPos = t.screenPosition; // origin bottom-left
            Vector2 guiPos = new Vector2(screenPos.x, Screen.height - screenPos.y); // IMGUI origin top-left

            if (t.phase == TouchPhase.Ended)
            {
                releasePoints.Add(guiPos);
                if (dragging && draggingTouchId == t.touchId)
                {
                    dragging = false;
                    draggingTouchId = -1;
                }
            }

            if (t.phase == TouchPhase.Began)
            {
                if (IsPointInFloatCircle(guiPos))
                {
                    dragging = true;
                    draggingTouchId = t.touchId;
                    dragStartTouchGui = guiPos;
                    dragStartWindowPos = new Vector2(floatRect.x, floatRect.y);
                }
            }
            else if ((t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary) && dragging && draggingTouchId == t.touchId)
            {
                Vector2 delta = guiPos - dragStartTouchGui;
                floatRect.x = dragStartWindowPos.x + delta.x;
                floatRect.y = dragStartWindowPos.y + delta.y;
            }
        }

        // mouse support (editor)
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                Vector2 guiPos = new Vector2(mousePos.x, Screen.height - mousePos.y);
                releasePoints.Add(guiPos);
                if (dragging && draggingTouchId == -1)
                {
                    dragging = false;
                }
            }
            if (Mouse.current.leftButton.isPressed)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                Vector2 guiPos = new Vector2(mousePos.x, Screen.height - mousePos.y);
                if (!dragging && IsPointInFloatCircle(guiPos))
                {
                    dragging = true;
                    draggingTouchId = -1;
                    dragStartTouchGui = guiPos;
                    dragStartWindowPos = new Vector2(floatRect.x, floatRect.y);
                }
                else if (dragging && draggingTouchId == -1)
                {
                    Vector2 delta = guiPos - dragStartTouchGui;
                    floatRect.x = dragStartWindowPos.x + delta.x;
                    floatRect.y = dragStartWindowPos.y + delta.y;
                }
            }
        }

        ClampRectsToSafeArea();
    }

    private void OnGUI()
    {
        GUI.depth = -100000;

        // DRAW expanded window first (if expanded)
        if (isExpanded)
        {
            expandedRect = GUI.Window(expandedWindowId, expandedRect, DrawExpandedWindow, "Debug Console");
            GUI.FocusWindow(expandedWindowId);
        }

        // DRAW float window (always)
        floatRect = GUI.Window(floatWindowId, floatRect, DrawFloatWindow, GUIContent.none);

        // clear unconsumed release points on repaint (safety)
        if (Event.current.type == EventType.Repaint)
        {
            releasePoints.Clear();
        }
    }

    private void DrawFloatWindow(int id)
    {
        GUIStyle box = new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        GUI.Box(new Rect(0, 0, floatRect.width, floatRect.height), GUIContent.none);
        GUI.Label(new Rect(0, 0, floatRect.width, floatRect.height), "DBG", box);

        // allow mouse drag (touch drag handled in Update)
        GUI.DragWindow(new Rect(0, 0, floatRect.width, floatRect.height));

        // handle touch releases: if any release point is inside the float circle -> toggle expand
        for (int i = releasePoints.Count - 1; i >= 0; --i)
        {
            if (IsPointInFloatCircle(releasePoints[i]))
            {
                releasePoints.RemoveAt(i);
                isExpanded = !isExpanded;
                ClampRectsToSafeArea();
                break;
            }
        }
    }

    private void DrawExpandedWindow(int id)
    {
        float padding = 8f;
        float headerH = 28f;
        float btnW = 70f;
        float btnH = 24f;
        float spacing = 6f;

        Rect headerLocal = new Rect(padding, padding, expandedRect.width - padding * 2f, headerH);

        // title
        Rect titleLocal = new Rect(headerLocal.x, headerLocal.y, headerLocal.width - (btnW + spacing) * 3f, headerLocal.height);
        GUI.Label(titleLocal, $"Debug Console ({logs.Count})");

        // settings button
        Rect settingsLocal = new Rect(headerLocal.x + headerLocal.width - (btnW + spacing) * 3f,
            headerLocal.y + (headerLocal.height - btnH) * 0.5f, btnW, btnH);
        if (GUI.Button(settingsLocal, "Settings")) showSettings = !showSettings;
        Rect settingsGlobal = new Rect(expandedRect.x + settingsLocal.x, expandedRect.y + settingsLocal.y, settingsLocal.width, settingsLocal.height);
        if (CheckAndConsumeReleaseInGlobalRect(settingsGlobal)) showSettings = !showSettings;

        // clear button
        Rect clearLocal = new Rect(headerLocal.x + headerLocal.width - (btnW + spacing) * 2f,
            headerLocal.y + (headerLocal.height - btnH) * 0.5f, btnW, btnH);
        if (GUI.Button(clearLocal, "Clear")) ClearLogs();
        Rect clearGlobal = new Rect(expandedRect.x + clearLocal.x, expandedRect.y + clearLocal.y, clearLocal.width, clearLocal.height);
        if (CheckAndConsumeReleaseInGlobalRect(clearGlobal)) ClearLogs();

        // close button
        Rect closeLocal = new Rect(headerLocal.x + headerLocal.width - (btnW + spacing),
            headerLocal.y + (headerLocal.height - btnH) * 0.5f, btnW, btnH);
        if (GUI.Button(closeLocal, "Close")) isExpanded = false;
        Rect closeGlobal = new Rect(expandedRect.x + closeLocal.x, expandedRect.y + closeLocal.y, closeLocal.width, closeLocal.height);
        if (CheckAndConsumeReleaseInGlobalRect(closeGlobal)) isExpanded = false;

        float logsY = headerLocal.yMax + padding;

        // settings panel
        if (showSettings)
        {
            Rect settingsBox = new Rect(padding, logsY, expandedRect.width - padding * 2f, 80f);
            GUI.Box(settingsBox, "Settings");

            Rect fontLabel = new Rect(settingsBox.x + 6f, settingsBox.y + 6f, 80f, 20f);
            GUI.Label(fontLabel, "Font Size");
            fontSize = (int)GUI.HorizontalSlider(
                new Rect(fontLabel.x + 90f, fontLabel.y, settingsBox.width - 100f, 20f),
                fontSize, 10, 28
            );

            Rect spacingLabel = new Rect(settingsBox.x + 6f, fontLabel.y + 26f, 80f, 20f);
            GUI.Label(spacingLabel, "Line Spacing");
            lineSpacing = GUI.HorizontalSlider(
                new Rect(spacingLabel.x + 90f, spacingLabel.y, settingsBox.width - 100f, 20f),
                lineSpacing, 0f, 20f
            );

            logsY = settingsBox.yMax + padding;
        }

        // Logs area
        Rect logsVisibleLocal = new Rect(padding, logsY, expandedRect.width - padding * 2f, expandedRect.height - logsY - padding);
        GUI.Box(logsVisibleLocal, GUIContent.none);

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = fontSize;

        float lineHeight = Mathf.Max(16f, labelStyle.lineHeight);
        float contentHeight = logs.Count * (lineHeight + lineSpacing);
        Rect contentLocal = new Rect(0, 0, logsVisibleLocal.width - 12f, contentHeight);

        scrollPos = GUI.BeginScrollView(logsVisibleLocal, scrollPos, contentLocal);

        for (int i = 0; i < logs.Count; ++i)
        {
            float y = i * (lineHeight + lineSpacing);
            Rect lineLocal = new Rect(4f, y, contentLocal.width - 4f, lineHeight);
            GUI.Label(lineLocal, logs[i], labelStyle);
        }

        GUI.EndScrollView();

        // drag by title area
        GUI.DragWindow(new Rect(0, 0, expandedRect.width, titleBarHeight));
    }

    private bool CheckAndConsumeReleaseInGlobalRect(Rect globalRect)
    {
        for (int i = releasePoints.Count - 1; i >= 0; --i)
        {
            if (globalRect.Contains(releasePoints[i]))
            {
                releasePoints.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    private bool IsPointInFloatCircle(Vector2 guiPoint)
    {
        Vector2 center = new Vector2(floatRect.x + floatRect.width * 0.5f, floatRect.y + floatRect.height * 0.5f);
        float radius = Mathf.Min(floatRect.width, floatRect.height) * 0.5f;
        return Vector2.SqrMagnitude(guiPoint - center) <= radius * radius;
    }

    private void ClearLogs()
    {
        logs.Clear();
        scrollPos = Vector2.zero;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string formatted = $"[{type}] {logString}";
        logs.Add(formatted);
        if (logs.Count > maxLogs) logs.RemoveAt(0);

        if (isExpanded)
            scrollPos.y = float.MaxValue;
    }

    private void ClampRectsToSafeArea()
    {
        Rect safe = Screen.safeArea;

        float w = Mathf.Clamp(floatRect.width, 32f, safe.width);
        float h = Mathf.Clamp(floatRect.height, 32f, safe.height);
        floatRect.x = Mathf.Clamp(floatRect.x, safe.xMin, safe.xMax - w);
        floatRect.y = Mathf.Clamp(floatRect.y, safe.yMin, safe.yMax - h);
        floatRect.width = w;
        floatRect.height = h;

        float exW = Mathf.Clamp(expandedRect.width, 120f, safe.width);
        float exH = Mathf.Clamp(expandedRect.height, titleBarHeight + 80f, safe.height);
        expandedRect.x = Mathf.Clamp(expandedRect.x, safe.xMin, safe.xMax - exW);
        expandedRect.y = Mathf.Clamp(expandedRect.y, safe.yMin, safe.yMax - exH);
        expandedRect.width = exW;
        expandedRect.height = exH;
    }

    // external API
    public void ShowPanel(bool show)
    {
        isExpanded = show;
        ClampRectsToSafeArea();
    }

    public bool IsPanelVisible() => isExpanded;
}
