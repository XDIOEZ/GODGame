using UnityEngine;

public class SpaceSceneManager : MonoBehaviour
{
    [Header("玩家引用")]
    public Transform player;

    [Header("横向循环背景 (3块拼接)")]
    public Transform[] horizontalTiles; // 必须填 3 个
    public float horizontalTileWidth = 20f;

    [Header("纵向循环背景 (3块拼接)")]
    public Transform[] verticalTiles; // 必须填 3 个
    public float verticalTileHeight = 20f;

    [Header("太空场景边界")]
    public float minY = 100f; // 太空场景的最低高度 (切回地面用)

    // --- 内部状态 ---
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (player == null || cam == null) return;

        HandleHorizontalLoop();
        HandleVerticalLoop();

        // 检查是否低于太空起点
        if (player.position.y < minY)
        {
            // 通知切换管理器
            SceneSwitcher switcher = FindObjectOfType<SceneSwitcher>();
            if (switcher != null)
            {
                switcher.EnterGround();
            }
        }
    }

    void HandleHorizontalLoop()
    {
        float camX = cam.position.x;

        foreach (Transform tile in horizontalTiles)
        {
            float dist = camX - tile.position.x;
            if (Mathf.Abs(dist) >= horizontalTileWidth * 1.5f)
            {
                float offset = Mathf.Sign(dist) * horizontalTileWidth * 3f;
                tile.position += new Vector3(offset, 0f, 0f);
            }
        }
    }

    void HandleVerticalLoop()
    {
        float camY = cam.position.y;

        foreach (Transform tile in verticalTiles)
        {
            float dist = camY - tile.position.y;
            if (Mathf.Abs(dist) >= verticalTileHeight * 1.5f)
            {
                float offset = Mathf.Sign(dist) * verticalTileHeight * 3f;
                tile.position += new Vector3(0f, offset, 0f);
            }
        }
    }
}
