using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class StartSceneUI : BasePanel
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

    private void OpenNewView()
    {
        UIManager.Instance.Show(UIPanelType.SecendView);
    }

    private void CloseSelf()
    {
        this.DestroySelf();
    }

    public void OnContinueGame()
    {
        Debug.Log("Continue Game button clicked");
        // Add logic to continue the game
        //测试后续删除
        GameManager.Instance.ContinueGame();
    }
    public void OnNewGame()
    {
        Debug.Log("New Game button clicked");
        // Add logic to start a new game
        //跳转到游戏场景
        GameManager.Instance.StartNewGame();
    }
    public void OnQuitGame()
    {
        Debug.Log("Quit Game button clicked");
        //#if UNITY_ANDROID              
        //// Android退回后台
        //AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        //#endif
        //activity.Call<bool>("moveTaskToBack", true);
        Application.Quit();
        // Add logic to quit the game
    }
}
