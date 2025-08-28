using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketChange : MonoBehaviour
{
    // ��Ҫ��Inspector��ָ����UI���
    [SerializeField] private GameObject targetUIPanel;

    // ֻ����BoxCollider2D
    [SerializeField] private BoxCollider2D targetBoxCollider2D;
    public Canvas canvas;

    // ���ڸ����Ƿ��Ѵ������ʵ��
    private GameObject panelInstance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // �����ײ
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ֻ���ָ����BoxCollider2D
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
