using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [Tooltip("UI�ı������������ʾ֡��")]
    public Text fpsText;
    public Text maxFps;

    // ����֡�ʵ�ʱ�������룩
    private const float updateInterval = 0.5f;
    // ʱ���ۼ���
    private float accumulator = 0f;
    // ֡��������
    private int frameCount = 0;
    // ��һ�θ��µ�ʱ��
    private float timeLeft = 0f;
    // ��ǰ֡��
    private float currentFps = 0f;

    void Start()
    {
        // ��ʼ��ʱ��
        timeLeft = updateInterval;

        // ���û��ָ��UI�ı����Զ�����һ��
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

        // �ﵽ���¼��ʱ����֡��
        if (timeLeft <= 0f)
        {
            currentFps = accumulator / frameCount;
            timeLeft = updateInterval;
            accumulator = 0f;
            frameCount = 0;

            // ��ʾ֡�ʣ�����һλС��
            fpsText.text = $"FPS: {currentFps:0.0}";

            // ����֡�ʸߵ͸ı���ɫ��ֱ�۷�����
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

    // �ṩ�ⲿ��ȡ��ǰ֡�ʵķ���
    public float GetCurrentFPS()
    {
        Application.targetFrameRate = -1;
        return currentFps;
    }
}
