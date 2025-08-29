using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TapSDK.Compliance;
using TapSDK.Core;
using TapSDK.Login;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TaptapLogin : MonoBehaviour
{

    private Button OnLoginbutton;

    private Button LogOutbutton;


    private TapADN TapADN;
    // Start is called before the first frame update

    private void Awake()
    {
        //OnLoginbutton = GameObject.Find("OnLoginbutton").GetComponent<Button>();

        //LogOutbutton = GameObject.Find("LogOutbutton").GetComponent<Button>();

        //OnLoginbutton.onClick.AddListener(OnLoginButton);

        //LogOutbutton.onClick.AddListener(OnLogOutButton);

    }

    void Start()
    {
    }

    private void OnEnable()
    {
        EventManager.Instance.On<EmptyEventTaptapLogin>(OnLoginButton);
    }

    private void OnDisable()
    {
        EventManager.Instance.Off<EmptyEventTaptapLogin>(OnLoginButton);
    }

    // Update is called once per frame
    void Update()
    {

      
    }

    

    private void OnLoginButton(EmptyEventTaptapLogin evt)
    {
       var i_0 = CheckLoginStatus();
    }

    /// <summary>
    /// �˳���¼
    /// </summary>
    private void OnLogOutButton()
    {
        TapTapLogin.Instance.Logout();
        TapTapCompliance.Exit();
    }

    /// <summary>
    /// �û���¼�ӿ�
    /// </summary>
    /// <returns></returns>
    private async Task UserLogingInAsync()
    {
        try
        {
            // ������Ȩ��Χ
            List<string> scopes = new List<string>
            {
                TapTapLogin.TAP_LOGIN_SCOPE_PUBLIC_PROFILE
            };
            // ���� Tap ��¼
            var userInfo = await TapTapLogin.Instance.LoginWithScopes(scopes.ToArray());
            TapADN.SideInit();
            Debug.Log($"��¼�ɹ�����ǰ�û� ID��{userInfo.unionId}");
        }
        catch (TaskCanceledException)
        {
            Debug.Log("�û�ȡ����¼");
        }
        catch (Exception exception)
        {
            Debug.Log($"��¼ʧ�ܣ������쳣��{exception}");
        }
    }
    /// <summary>
    /// �û���Ϣ��ӡ
    /// </summary>
    /// <returns></returns>
    private async Task LogoUsetinformationAsync()
    {
        TapTapAccount taptapAccount = await TapTapLogin.Instance.GetCurrentTapAccount();
        AccessToken accessToken = taptapAccount.accessToken;
        string openId = taptapAccount.openId;
        string userName = taptapAccount.name;


        if (taptapAccount != null)
        {
            string userIdentifier = taptapAccount.unionId;
            TapTapCompliance.Startup(userIdentifier);
        }
        Debug.Log("��ҵ�accessToken:"+accessToken);
        Debug.Log("��ҵ�openId:" + openId);
        Debug.Log("��ҵ�accessToken:" + userName);
    }

    /// <summary>
    /// Tap�˼���û��Ƿ��¼--��bool
    /// </summary>
    /// <returns></returns>
    private  async Task<bool> CheckUserState()
    {
        try
        {
            TapTapAccount account = await TapTapLogin.Instance.GetCurrentTapAccount();
            if (account == null)
            {
                // �û�δ��¼
                Debug.Log("��ǰ�û�δ��¼");
                return false;
            }
            else
            {
                // �û��ѵ�¼
                Debug.Log("��ǰ�û��ѵ�¼");
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"��ȡ�û���Ϣʧ�� {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// �ж��Ƿ���е�¼�ӿ�
    /// </summary>
    /// <returns></returns>
    private async Task CheckLoginStatus()
    {
    // ʹ�� await ��ȡʵ�ʵ� bool ֵ
    bool isLoggedIn = await CheckUserState();
    
    if (isLoggedIn)
    {
        Debug.Log("�û��ѵ�¼");
            // ִ���ѵ�¼�߼�
            var i_2 = LogoUsetinformationAsync();
        }
    else
    {
        Debug.Log("�û�δ��¼");
            // ִ�е�¼����
            var i_3 = UserLogingInAsync();
        }
    }

    /// <summary>
    ///  ����У�顪������������ר��
    /// </summary>
    /// <returns></returns>
    private async Task Startverification()
    {
        bool IsSuccess = await TapTapSDK.IsLaunchedFromTapTapPC();
        if (IsSuccess)
        {
            UnityEngine.Debug.Log(" TapTap PC ��У��ͨ��");
            // TODO: ����������¼������ҵ������
            var i_1 = CheckLoginStatus();
        }
        else
        {
            UnityEngine.Debug.Log(" TapTap PC ��У��δͨ��");
            // ִֹͣ�к���ҵ������
        }
    }
}
