using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketChange : MonoBehaviour
{
    // 需要在Inspector中指定的UI面板
    [SerializeField] private GameObject targetUIPanel;

    // 只检测此BoxCollider2D
    [SerializeField] private BoxCollider2D targetBoxCollider2D;
    public Canvas canvas;

    // 用于跟踪是否已创建面板实例
    private GameObject panelInstance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // 检测碰撞
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 只检测指定的BoxCollider2D
        if (targetBoxCollider2D != null && other == targetBoxCollider2D)
        {
            if (targetUIPanel != null && panelInstance == null)
            {
                targetUIPanel.SetActive(true);
                Debug.Log("RocketChange: Target UI Panel activated.");
                panelInstance = RocketChangeSceneController.Instantiate(targetUIPanel, canvas.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
