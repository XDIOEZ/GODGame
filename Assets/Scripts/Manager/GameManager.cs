using UnityEngine;

public class GameManager : SingletonAutoMono<GameManager>
{
    [Header("玩家引用")]
    private Player player;  // 玩家对象引用

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

    [Header("场景配置")]
    [Tooltip("新游戏的场景名称")]
    [SerializeField] private string startGameScene = "飞船控制器";
    [Tooltip("继续游戏的场景名称")]
    [SerializeField] private string continueGameScene = "飞船控制器";
    [Tooltip("暂停时切换的场景名称")]
    [SerializeField] private string pauseGameScene = "UI面板测试";

    private new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject); // 场景切换不销毁
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartNewGame()
    {
        Debug.Log("游戏开始！");
        UnityEngine.SceneManagement.SceneManager.LoadScene(startGameScene);
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ContinueGame()
    {
        Debug.Log("继续游戏！");
        UnityEngine.SceneManagement.SceneManager.LoadScene(continueGameScene);
    }

    /// <summary>
    /// 暂停/继续游戏
    /// </summary>
    public void PauseGame(bool continueGame)
    {
        Debug.Log("切换到暂停场景！");
        UnityEngine.SceneManagement.SceneManager.LoadScene(pauseGameScene);
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    public void EndGame()
    {
        Debug.Log("游戏结束！");
        // 可以在这里加载游戏结束界面
    }
}
