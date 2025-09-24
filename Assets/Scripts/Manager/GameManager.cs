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
    private void Start()
{
    // 每60秒自动保存一次游戏时间
    InvokeRepeating(nameof(SavePlayTimePeriodically), 60f, 60f);
}

private void SavePlayTimePeriodically()
{
    PlayerPlayTimeManager.SavePlayerPlayTime();
}

private void OnApplicationPause(bool pauseStatus)
{
    // 当应用暂停时保存游戏时间
    if (pauseStatus)
    {
        PlayerPlayTimeManager.SavePlayerPlayTime();
    }
}

private void OnApplicationFocus(bool hasFocus)
{
    // 当应用失去焦点时保存游戏时间
    if (!hasFocus)
    {
        PlayerPlayTimeManager.SavePlayerPlayTime();
    }
}

private void OnApplicationQuit()
{
    // 当应用退出时保存游戏时间
    PlayerPlayTimeManager.SavePlayerPlayTime();
}

/// <summary>
/// 开始游戏
/// </summary>
public void StartNewGame()
{
    Debug.Log("游戏开始！");
    Checkpoint.LoadPlayerDeathCount();   // 从存储中恢复死亡次数
    PlayerPlayTimeManager.LoadPlayerPlayTime(); // 从存储中恢复游戏时间
    UnityEngine.SceneManagement.SceneManager.LoadScene(startGameScene);
}

/// <summary>
/// 结束游戏
/// </summary>
public void EndGame()
{
    Debug.Log("游戏结束！");
    PlayerPlayTimeManager.SavePlayerPlayTime(); // 保存游戏时间到存储
    // 可以在这里加载游戏结束界面
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
}
/// <summary>
/// 玩家游戏时间管理类
/// </summary>
public static class PlayerPlayTimeManager
{
    private static float playerTotalPlayTime = 0f; // 玩家总游戏时间（秒）
    private static float sessionStartTime = 0f;    // 本次游戏会话开始时间

    /// <summary>
    /// 从持久化存储中恢复玩家总游戏时间
    /// </summary>
    public static void LoadPlayerPlayTime()
    {
        playerTotalPlayTime = PlayerPrefs.GetFloat("PlayerTotalPlayTime", 0f);
        sessionStartTime = Time.time; // 记录本次游戏会话开始时间
        Debug.Log($"加载玩家游戏时间: {GetFormattedPlayTime()}");
    }

    /// <summary>
    /// 保存玩家总游戏时间到持久化存储
    /// </summary>
    public static void SavePlayerPlayTime()
    {
        // 累加本次游戏会话的时间
        float sessionPlayTime = Time.time - sessionStartTime;
        playerTotalPlayTime += sessionPlayTime;
        
        PlayerPrefs.SetFloat("PlayerTotalPlayTime", playerTotalPlayTime);
        PlayerPrefs.Save();
        
        Debug.Log($"保存玩家游戏时间: {GetFormattedPlayTime()}");
    }

    /// <summary>
    /// 获取玩家总游戏时间（包含本次会话时间）
    /// </summary>
    /// <returns>总游戏时间(秒)</returns>
    public static float GetPlayerTotalPlayTime()
    {
        // 包含本次游戏会话的时间
        float sessionPlayTime = Time.time - sessionStartTime;
        return playerTotalPlayTime + sessionPlayTime;
    }

    /// <summary>
    /// 获取格式化的时间字符串
    /// </summary>
    /// <returns>格式化的时间字符串 (小时:分钟:秒)</returns>
    public static string GetFormattedPlayTime()
    {
        float totalSeconds = GetPlayerTotalPlayTime();
        int hours = Mathf.FloorToInt(totalSeconds / 3600);
        int minutes = Mathf.FloorToInt((totalSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    /// <summary>
    /// 重置会话开始时间（在加载新场景时调用）
    /// </summary>
    public static void ResetSessionStartTime()
    {
        sessionStartTime = Time.time;
    }
}