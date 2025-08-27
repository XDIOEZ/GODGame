using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public Transform player;

    [Header("地面场景")]
    public GameObject groundScene;
    public float groundToSpaceHeight = 100f; // 高度超过100切到太空

    [Header("太空场景")]
    public GameObject spaceScene;
    public float spaceMinHeight = 100f; // 太空场景的最低高度 = groundToSpaceHeight

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
        // 你也可以在这里调整玩家位置，让它无缝衔接
    }

    public void EnterGround()
    {
        inSpace = false;
        groundScene.SetActive(true);
        spaceScene.SetActive(false);
    }
}
