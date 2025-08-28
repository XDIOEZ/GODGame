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
        // 还可以初始化关卡、重置分数等
         UnityEngine.SceneManagement.SceneManager.LoadScene("飞船控制器");
        
    }
    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ContinueGame()
    {
        Debug.Log("游戏开始！");
        // 还可以初始化关卡、重置分数等
        UnityEngine.SceneManagement.SceneManager.LoadScene("飞船控制器");

    }


    /// <summary>
    /// 暂停/继续游戏
    /// </summary>
    public void PauseGame(bool continueGame)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI面板测试");
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
