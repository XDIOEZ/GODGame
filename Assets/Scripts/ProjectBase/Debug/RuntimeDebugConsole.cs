using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuntimeDebugConsole : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput; // 同对象上的 PlayerInput
    [SerializeField] private string actionName = "ShowDebugUI"; // InputAction 名称

    private bool showConsole = false;
    private Vector2 scrollPosition;
    private readonly List<string> logs = new List<string>();
    private const int maxLogs = 200; // 最多保留多少条日志

    private InputAction debugAction;

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;

        // 原 PlayerInput Action 逻辑
        if (playerInput != null)
        {
            debugAction = playerInput.actions[actionName];
            if (debugAction != null)
            {
                debugAction.performed += OnDebugAction;
                debugAction.Enable();
            }
            else
            {
                Debug.LogWarning($"PlayerInput 上未找到 InputAction: {actionName}");
            }
        }
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;

        if (debugAction != null)
        {
            debugAction.performed -= OnDebugAction;
            debugAction.Disable();
        }
    }

    private void OnDebugAction(InputAction.CallbackContext context)
    {
        ToggleConsole();
    }

    private void ToggleConsole()
    {
        showConsole = !showConsole;
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height / 2), "Debug Console");

        GUILayout.BeginArea(new Rect(20, 40, Screen.width - 40, Screen.height / 2 - 50));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var log in logs)
        {
            GUILayout.Label(log);
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string formattedLog = $"[{type}] {logString}";
        logs.Add(formattedLog);

        if (logs.Count > maxLogs)
            logs.RemoveAt(0); // 移除最旧的日志
    }

    private void Update()
    {
//#if UNITY_ANDROID && !UNITY_EDITOR
//        // Android 上监听音量键同时按下
//        if (Input.GetKey(KeyCode.VolumeUp) && Input.GetKey(KeyCode.VolumeDown))
//        {
//            ToggleConsole();
//        }
//#endif
    }
}
