using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class RocketChangeSceneController : BasePanel
{
    public Button backButton;
    [Header("�Զ������߷���㣨��Ŀ������һ�£�")]
    public List<Transform> raycastOrigins;

    [Header("��Ҫ���ӵ�Ŀ������")]
    public List<Transform> targets;

    [Header("�����õ�LineRendererԤ����")]
    public LineRenderer linePrefab;

    private List<LineRenderer> lines = new List<LineRenderer>();
    private float previousTimeScale = 1f;

    void Start()
    {
        // ��ͣ��Ϸʱ��
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        DrawLines();
        backButton.onClick.AddListener(() =>
        {
            // �ָ���Ϸʱ��
            Time.timeScale = previousTimeScale;
            this.OnExit();
            print("���ذ�ť�����");
        });
    }

    void DrawLines()
    {
        // �������
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
