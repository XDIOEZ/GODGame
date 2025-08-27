using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public Transform player;

    [Header("���泡��")]
    public GameObject groundScene;
    public float groundToSpaceHeight = 100f; // �߶ȳ���100�е�̫��

    [Header("̫�ճ���")]
    public GameObject spaceScene;
    public float spaceMinHeight = 100f; // ̫�ճ�������͸߶� = groundToSpaceHeight

    private bool inSpace = false;

    void Update()
    {
        if (!inSpace && player.position.y >= groundToSpaceHeight)
        {
            EnterSpace();
        }
        else if (inSpace && player.position.y < spaceMinHeight)
        {
            EnterGround();
        }
    }

    public void EnterSpace()
    {
        inSpace = true;
        groundScene.SetActive(false);
        spaceScene.SetActive(true);
        // ��Ҳ����������������λ�ã������޷��ν�
    }

    public void EnterGround()
    {
        inSpace = false;
        groundScene.SetActive(true);
        spaceScene.SetActive(false);
    }
}
