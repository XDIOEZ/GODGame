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
        // �����Գ�ʼ���ؿ������÷�����
         UnityEngine.SceneManagement.SceneManager.LoadScene("�ɴ�������");
        
    }
    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void ContinueGame()
    {
        Debug.Log("��Ϸ��ʼ��");
        // �����Գ�ʼ���ؿ������÷�����
        UnityEngine.SceneManagement.SceneManager.LoadScene("�ɴ�������");

    }


    /// <summary>
    /// ��ͣ/������Ϸ
    /// </summary>
    public void PauseGame(bool continueGame)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI������");
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
