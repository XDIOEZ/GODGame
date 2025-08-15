using System;
using System.Collections;
using System.Collections.Generic;
using TapSDK.Compliance;
using TapSDK.Core;
using UnityEngine;

public class TaptapInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TapTapSdkOptions coreOptions = new TapTapSdkOptions
        {
            // 客户端 ID，开发者后台获取
            clientId = "j6qukxgc7zcndtzs7a",
            // 客户端令牌，开发者后台获取
            clientToken = "f2XhbB95F6ohfNtH1BWv6j2Bh6pZRVhQIf4X1LHk",
            // 地区，CN 为国内，Overseas 为海外
            region = TapTapRegionType.CN,
            // 语言，默认为 Auto，默认情况下，国内为 zh_Hans，海外为 en
            preferredLanguage = TapTapLanguageType.zh_Hans,
            // 是否开启日志，Release 版本请设置为 false
            enableLog = true
        };
        TapTapComplianceOption complianceOption = new TapTapComplianceOption
        {
            showSwitchAccount = true,
            useAgeRange = true
        };
        // 当需要添加其他模块的初始化配置项，例如合规认证、成就等， 请使用如下 API
        TapTapSdkBaseOptions[] otherOptions = new TapTapSdkBaseOptions[]
        {
            // 其他模块配置项
             complianceOption
        };

        TapTapSDK.Init(coreOptions, otherOptions);
        RegisterComplianceCallback();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RegisterComplianceCallback()
    {
        Action<int, string> callback = (code, s) =>
        {
            Debug.Log($"合规回调: Code={code}, Message={s}");
            HandleComplianceResult(code, s);
        };

        TapTapCompliance.RegisterComplianceCallback(callback);
    }

    private void HandleComplianceResult(int code, string message)
    {
        // 处理合规检查结果
        switch (code)
        {
            case 0: // 合规检查通过
                Debug.Log("合规检查通过，开始游戏");

                break;
            case 1: // 需要实名认证
                Debug.Log("需要实名认证");

                break;
            case 2: // 未成年用户
                Debug.Log("未成年用户，应用防沉迷策略");

                break;
            default:
                Debug.LogWarning($"未知合规状态码: {code}");
                break;
        }
    }

}
