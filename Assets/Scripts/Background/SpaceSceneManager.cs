using UnityEngine;

public class SpaceSceneManager : MonoBehaviour
{
    [Header("�������")]
    public Transform player;

    [Header("����ѭ������ (3��ƴ��)")]
    public Transform[] horizontalTiles; // ������ 3 ��
    public float horizontalTileWidth = 20f;

    [Header("����ѭ������ (3��ƴ��)")]
    public Transform[] verticalTiles; // ������ 3 ��
    public float verticalTileHeight = 20f;

    [Header("̫�ճ����߽�")]
    public float minY = 100f; // ̫�ճ�������͸߶� (�лص�����)

    // --- �ڲ�״̬ ---
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

        // ����Ƿ����̫�����
        if (player.position.y < minY)
        {
            // ֪ͨ�л�������
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
