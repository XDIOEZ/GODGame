using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    public Button continueGameBtn;
    public Button newGameBtn;
    public Button quitBtn;



    // Start is called before the first frame update
    void Start()
    {
        continueGameBtn.onClick.AddListener(OnContinueGame);
        newGameBtn.onClick.AddListener(OnNewGame);
        quitBtn.onClick.AddListener(OnQuitGame);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnContinueGame()
    {
        Debug.Log("Continue Game button clicked");
        // Add logic to continue the game
        //测试后续删除
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI面板测试");
    }
    public void OnNewGame()
    {
        Debug.Log("New Game button clicked");
        // Add logic to start a new game
        //跳转到游戏场景
        UnityEngine.SceneManagement.SceneManager.LoadScene("飞船控制器");
    }
    public void OnQuitGame()
    {
        Debug.Log("Quit Game button clicked");
        // Add logic to quit the game
        Application.Quit();
    }
}
