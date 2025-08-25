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
        //���Ժ���ɾ��
        UnityEngine.SceneManagement.SceneManager.LoadScene("UI������");
    }
    public void OnNewGame()
    {
        Debug.Log("New Game button clicked");
        // Add logic to start a new game
        //��ת����Ϸ����
        UnityEngine.SceneManagement.SceneManager.LoadScene("�ɴ�������");
    }
    public void OnQuitGame()
    {
        Debug.Log("Quit Game button clicked");
        // Add logic to quit the game
        Application.Quit();
    }
}
