using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingView : BasePanel
{
    [SerializeField]
    private SerializableDictionary<Button, GameObject> SettingDic = new SerializableDictionary<Button, GameObject>();

    public Button closeBtn;
    public Button backMainMenuBtn;
    // Start is called before the first frame update
    void Start()
    {
        BtnAddlistInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    { 

    }

    private void BtnAddlistInit()
    {
        closeBtn.onClick.AddListener(() =>{ OnExit(); });
        backMainMenuBtn.onClick.AddListener(() => { UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene"); });

        foreach (var pair in SettingDic)
        {
            // 关键：用临时变量存储当前循环的 Button 和 GameObject（避免闭包陷阱）
            Button currentBtn = pair.Key;
            GameObject currentGO = pair.Value;

            currentBtn.onClick.AddListener(() =>
            {
                // 1. 遍历所有 Button-GameObject 对
                foreach (var item in SettingDic)
                {
                    // 2. 区分“当前点击的按钮”和“其他按钮”
                    if (item.Key == currentBtn)
                    {
                        // 显示当前按钮对应的 GameObject
                        item.Value.SetActive(true);
                    }
                    else
                    {
                        // 关闭其他按钮对应的 GameObject
                        item.Value.SetActive(false);
                    }
                }
            });
        }

        CheckCurrentScene();
    }
    void CheckCurrentScene()
    {
        // 获取当前激活的场景
        Scene currentScene = SceneManager.GetActiveScene();

        // 获取场景名称
        string sceneName = currentScene.name;

        // 判断是否为目标场景（例如 "MainScene"）
        if (sceneName == "StartScene")
        {
            backMainMenuBtn.gameObject.SetActive(false);
            Debug.Log("当前是开始场景");
        }
        else {
            backMainMenuBtn.gameObject.SetActive(true);
        }
    }
}
