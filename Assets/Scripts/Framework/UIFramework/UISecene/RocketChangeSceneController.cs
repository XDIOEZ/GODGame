using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class RocketChangeSceneController : BasePanel
{
    public Button backButton;
    [Header("自定义射线发射点（与目标数量一致）")]
    public List<Transform> raycastOrigins;

    [Header("需要连接的目标物体")]
    public List<Transform> targets;

    [Header("画线用的LineRenderer预制体")]
    public LineRenderer linePrefab;

    private List<LineRenderer> lines = new List<LineRenderer>();
    private float previousTimeScale = 1f;

    void Start()
    {
        // 暂停游戏时间
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        DrawLines();
        backButton.onClick.AddListener(() =>
        {
            // 恢复游戏时间
            Time.timeScale = previousTimeScale;
            this.OnExit();
            print("返回按钮被点击");
        });
    }

    void DrawLines()
    {
        // 清理旧线
        foreach (var line in lines)
        {
            if (line != null) Destroy(line.gameObject);
        }
        lines.Clear();

        int count = Mathf.Min(raycastOrigins.Count, targets.Count);
        for (int i = 0; i < count; i++)
        {
            var origin = raycastOrigins[i];
            var target = targets[i];
            if (origin == null || target == null) continue;

            var line = Instantiate(linePrefab, transform);
            lines.Add(line);

            line.positionCount = 2;
            line.SetPosition(0, origin.position);
            line.SetPosition(1, target.position);
        }
    }

    void Update()
    {
        DrawLines();
    }
}
