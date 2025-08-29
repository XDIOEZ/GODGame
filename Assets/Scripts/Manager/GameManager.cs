using UnityEngine;

public class GameManager : SingletonAutoMono<GameManager>
{
    [Header("�������")]
    private Player player;  // ��Ҷ�������

    public Player Player
    {
        get
        {
            if (player == null)
            {
                player = FindObjectOfType<Player>();
            }
            return player;
        }
        set
        {
            player = value;
        }
    }

    [Header("��������")]
    [Tooltip("����Ϸ�ĳ�������")]
    [SerializeField] private string startGameScene = "�ɴ�������";
    [Tooltip("������Ϸ�ĳ�������")]
    [SerializeField] private string continueGameScene = "�ɴ�������";
    [Tooltip("��ͣʱ�л��ĳ�������")]
    [SerializeField] private string pauseGameScene = "UI������";

    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); // �����л�������
    }

    /// <summary>
    /// ��ʼ��Ϸ
    /// </summary>
    public void StartNewGame()
    {
        Debug.Log("��Ϸ��ʼ��");
        UnityEngine.SceneManagement.SceneManager.LoadScene(startGameScene);
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void ContinueGame()
    {
        Debug.Log("������Ϸ��");
        UnityEngine.SceneManagement.SceneManager.LoadScene(continueGameScene);
    }

    /// <summary>
    /// ��ͣ/������Ϸ
    /// </summary>
    public void PauseGame(bool continueGame)
    {
        Debug.Log("�л�����ͣ������");
        UnityEngine.SceneManagement.SceneManager.LoadScene(pauseGameScene);
    }

    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void EndGame()
    {
        Debug.Log("��Ϸ������");
        // ���������������Ϸ��������
    }
}
