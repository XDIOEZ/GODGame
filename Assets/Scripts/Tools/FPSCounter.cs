using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [Tooltip("UI文本组件，用于显示帧率")]
    public Text fpsText;
    public Text maxFps;

    // 计算帧率的时间间隔（秒）
    private const float updateInterval = 0.5f;
    // 时间累加器
    private float accumulator = 0f;
    // 帧数计数器
    private int frameCount = 0;
    // 上一次更新的时间
    private float timeLeft = 0f;
    // 当前帧率
    private float currentFps = 0f;

    void Start()
    {
        // 初始化时间
        timeLeft = updateInterval;

        // 如果没有指定UI文本，自动创建一个
        if (fpsText == null)
        {
            GameObject textObj = new GameObject("FPS Display");
            fpsText = textObj.AddComponent<Text>();
            Canvas canvas = FindObjectOfType<Canvas>();

            if (canvas == null)
            {
                canvas = new GameObject("Canvas").AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                new GameObject("EventSystem").AddComponent<UnityEngine.EventSystems.EventSystem>();
            }

            textObj.transform.SetParent(canvas.transform);
            fpsText.rectTransform.anchorMin = Vector2.zero;
            fpsText.rectTransform.anchorMax = Vector2.zero;
            fpsText.rectTransform.pivot = Vector2.zero;
            fpsText.rectTransform.anchoredPosition = new Vector2(10, Screen.height - 30);
            fpsText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            fpsText.fontSize = 24;
            fpsText.color = Color.green;
        }
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        accumulator += Time.timeScale / Time.deltaTime;
        frameCount++;

        // 达到更新间隔时计算帧率
        if (timeLeft <= 0f)
        {
            currentFps = accumulator / frameCount;
            timeLeft = updateInterval;
            accumulator = 0f;
            frameCount = 0;

            // 显示帧率，保留一位小数
            fpsText.text = $"FPS: {currentFps:0.0}";

            // 根据帧率高低改变颜色（直观反馈）
            if (currentFps < 30)
                fpsText.color = Color.red;
            else if (currentFps < 50)
                fpsText.color = Color.yellow;
            else
                fpsText.color = Color.green;
        }
        maxFps.text = $"MaxFPS: {Application.targetFrameRate}";
        Application.targetFrameRate =120;
    }

    // 提供外部获取当前帧率的方法
    public float GetCurrentFPS()
    {
        Application.targetFrameRate = -1;
        return currentFps;
    }
}
