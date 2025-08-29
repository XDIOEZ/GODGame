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
    /// 退出登录
    /// </summary>
    private void OnLogOutButton()
    {
        TapTapLogin.Instance.Logout();
        TapTapCompliance.Exit();
    }

    /// <summary>
    /// 用户登录接口
    /// </summary>
    /// <returns></returns>
    private async Task UserLogingInAsync()
    {
        try
        {
            // 定义授权范围
            List<string> scopes = new List<string>
            {
                TapTapLogin.TAP_LOGIN_SCOPE_PUBLIC_PROFILE
            };
            // 发起 Tap 登录
            var userInfo = await TapTapLogin.Instance.LoginWithScopes(scopes.ToArray());
            TapADN.SideInit();
            Debug.Log($"登录成功，当前用户 ID：{userInfo.unionId}");
        }
        catch (TaskCanceledException)
        {
            Debug.Log("用户取消登录");
        }
        catch (Exception exception)
        {
            Debug.Log($"登录失败，出现异常：{exception}");
        }
    }
    /// <summary>
    /// 用户信息打印
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
        Debug.Log("玩家的accessToken:"+accessToken);
        Debug.Log("玩家的openId:" + openId);
        Debug.Log("玩家的accessToken:" + userName);
    }

    /// <summary>
    /// Tap端检测用户是否登录--》bool
    /// </summary>
    /// <returns></returns>
    private  async Task<bool> CheckUserState()
    {
        try
        {
            TapTapAccount account = await TapTapLogin.Instance.GetCurrentTapAccount();
            if (account == null)
            {
                // 用户未登录
                Debug.Log("当前用户未登录");
                return false;
            }
            else
            {
                // 用户已登录
                Debug.Log("当前用户已登录");
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.Log($"获取用户信息失败 {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// 判断是否进行登录接口
    /// </summary>
    /// <returns></returns>
    private async Task CheckLoginStatus()
    {
    // 使用 await 获取实际的 bool 值
    bool isLoggedIn = await CheckUserState();
    
    if (isLoggedIn)
    {
        Debug.Log("用户已登录");
            // 执行已登录逻辑
            var i_2 = LogoUsetinformationAsync();
        }
    else
    {
        Debug.Log("用户未登录");
            // 执行登录流程
            var i_3 = UserLogingInAsync();
        }
    }

    /// <summary>
    ///  启动校验——》开发测试专用
    /// </summary>
    /// <returns></returns>
    private async Task Startverification()
    {
        bool IsSuccess = await TapTapSDK.IsLaunchedFromTapTapPC();
        if (IsSuccess)
        {
            UnityEngine.Debug.Log(" TapTap PC 端校验通过");
            // TODO: 继续后续登录等其他业务流程
            var i_1 = CheckLoginStatus();
        }
        else
        {
            UnityEngine.Debug.Log(" TapTap PC 端校验未通过");
            // 停止执行后续业务流程
        }
    }
}
