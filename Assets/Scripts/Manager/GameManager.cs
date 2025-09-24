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
    private void Start()
{
    // ÿ60���Զ�����һ����Ϸʱ��
    InvokeRepeating(nameof(SavePlayTimePeriodically), 60f, 60f);
}

private void SavePlayTimePeriodically()
{
    PlayerPlayTimeManager.SavePlayerPlayTime();
}

private void OnApplicationPause(bool pauseStatus)
{
    // ��Ӧ����ͣʱ������Ϸʱ��
    if (pauseStatus)
    {
        PlayerPlayTimeManager.SavePlayerPlayTime();
    }
}

private void OnApplicationFocus(bool hasFocus)
{
    // ��Ӧ��ʧȥ����ʱ������Ϸʱ��
    if (!hasFocus)
    {
        PlayerPlayTimeManager.SavePlayerPlayTime();
    }
}

private void OnApplicationQuit()
{
    // ��Ӧ���˳�ʱ������Ϸʱ��
    PlayerPlayTimeManager.SavePlayerPlayTime();
}

/// <summary>
/// ��ʼ��Ϸ
/// </summary>
public void StartNewGame()
{
    Debug.Log("��Ϸ��ʼ��");
    Checkpoint.LoadPlayerDeathCount();   // �Ӵ洢�лָ���������
    PlayerPlayTimeManager.LoadPlayerPlayTime(); // �Ӵ洢�лָ���Ϸʱ��
    UnityEngine.SceneManagement.SceneManager.LoadScene(startGameScene);
}

/// <summary>
/// ������Ϸ
/// </summary>
public void EndGame()
{
    Debug.Log("��Ϸ������");
    PlayerPlayTimeManager.SavePlayerPlayTime(); // ������Ϸʱ�䵽�洢
    // ���������������Ϸ��������
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
}
/// <summary>
/// �����Ϸʱ�������
/// </summary>
public static class PlayerPlayTimeManager
{
    private static float playerTotalPlayTime = 0f; // �������Ϸʱ�䣨�룩
    private static float sessionStartTime = 0f;    // ������Ϸ�Ự��ʼʱ��

    /// <summary>
    /// �ӳ־û��洢�лָ��������Ϸʱ��
    /// </summary>
    public static void LoadPlayerPlayTime()
    {
        playerTotalPlayTime = PlayerPrefs.GetFloat("PlayerTotalPlayTime", 0f);
        sessionStartTime = Time.time; // ��¼������Ϸ�Ự��ʼʱ��
        Debug.Log($"���������Ϸʱ��: {GetFormattedPlayTime()}");
    }

    /// <summary>
    /// �����������Ϸʱ�䵽�־û��洢
    /// </summary>
    public static void SavePlayerPlayTime()
    {
        // �ۼӱ�����Ϸ�Ự��ʱ��
        float sessionPlayTime = Time.time - sessionStartTime;
        playerTotalPlayTime += sessionPlayTime;
        
        PlayerPrefs.SetFloat("PlayerTotalPlayTime", playerTotalPlayTime);
        PlayerPrefs.Save();
        
        Debug.Log($"���������Ϸʱ��: {GetFormattedPlayTime()}");
    }

    /// <summary>
    /// ��ȡ�������Ϸʱ�䣨�������λỰʱ�䣩
    /// </summary>
    /// <returns>����Ϸʱ��(��)</returns>
    public static float GetPlayerTotalPlayTime()
    {
        // ����������Ϸ�Ự��ʱ��
        float sessionPlayTime = Time.time - sessionStartTime;
        return playerTotalPlayTime + sessionPlayTime;
    }

    /// <summary>
    /// ��ȡ��ʽ����ʱ���ַ���
    /// </summary>
    /// <returns>��ʽ����ʱ���ַ��� (Сʱ:����:��)</returns>
    public static string GetFormattedPlayTime()
    {
        float totalSeconds = GetPlayerTotalPlayTime();
        int hours = Mathf.FloorToInt(totalSeconds / 3600);
        int minutes = Mathf.FloorToInt((totalSeconds % 3600) / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        
        return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    /// <summary>
    /// ���ûỰ��ʼʱ�䣨�ڼ����³���ʱ���ã�
    /// </summary>
    public static void ResetSessionStartTime()
    {
        sessionStartTime = Time.time;
    }
}