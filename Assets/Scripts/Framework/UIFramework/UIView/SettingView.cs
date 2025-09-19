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
            // �ؼ�������ʱ�����洢��ǰѭ���� Button �� GameObject������հ����壩
            Button currentBtn = pair.Key;
            GameObject currentGO = pair.Value;

            currentBtn.onClick.AddListener(() =>
            {
                // 1. �������� Button-GameObject ��
                foreach (var item in SettingDic)
                {
                    // 2. ���֡���ǰ����İ�ť���͡�������ť��
                    if (item.Key == currentBtn)
                    {
                        // ��ʾ��ǰ��ť��Ӧ�� GameObject
                        item.Value.SetActive(true);
                    }
                    else
                    {
                        // �ر�������ť��Ӧ�� GameObject
                        item.Value.SetActive(false);
                    }
                }
            });
        }

        CheckCurrentScene();
    }
    void CheckCurrentScene()
    {
        // ��ȡ��ǰ����ĳ���
        Scene currentScene = SceneManager.GetActiveScene();

        // ��ȡ��������
        string sceneName = currentScene.name;

        // �ж��Ƿ�ΪĿ�곡�������� "MainScene"��
        if (sceneName == "StartScene")
        {
            backMainMenuBtn.gameObject.SetActive(false);
            Debug.Log("��ǰ�ǿ�ʼ����");
        }
        else {
            backMainMenuBtn.gameObject.SetActive(true);
        }
    }
}
