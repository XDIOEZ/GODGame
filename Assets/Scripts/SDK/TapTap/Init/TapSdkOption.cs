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
            // �ͻ��� ID�������ߺ�̨��ȡ
            clientId = "j6qukxgc7zcndtzs7a",
            // �ͻ������ƣ������ߺ�̨��ȡ
            clientToken = "f2XhbB95F6ohfNtH1BWv6j2Bh6pZRVhQIf4X1LHk",
            // ������CN Ϊ���ڣ�Overseas Ϊ����
            region = TapTapRegionType.CN,
            // ���ԣ�Ĭ��Ϊ Auto��Ĭ������£�����Ϊ zh_Hans������Ϊ en
            preferredLanguage = TapTapLanguageType.zh_Hans,
            // �Ƿ�����־��Release �汾������Ϊ false
            enableLog = true
        };
        TapTapComplianceOption complianceOption = new TapTapComplianceOption
        {
            showSwitchAccount = true,
            useAgeRange = true
        };
        // ����Ҫ�������ģ��ĳ�ʼ�����������Ϲ���֤���ɾ͵ȣ� ��ʹ������ API
        TapTapSdkBaseOptions[] otherOptions = new TapTapSdkBaseOptions[]
        {
            // ����ģ��������
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
            Debug.Log($"�Ϲ�ص�: Code={code}, Message={s}");
            HandleComplianceResult(code, s);
        };

        TapTapCompliance.RegisterComplianceCallback(callback);
    }

    private void HandleComplianceResult(int code, string message)
    {
        // ����Ϲ�����
        switch (code)
        {
            case 0: // �Ϲ���ͨ��
                Debug.Log("�Ϲ���ͨ������ʼ��Ϸ");

                break;
            case 1: // ��Ҫʵ����֤
                Debug.Log("��Ҫʵ����֤");

                break;
            case 2: // δ�����û�
                Debug.Log("δ�����û���Ӧ�÷����Բ���");

                break;
            default:
                Debug.LogWarning($"δ֪�Ϲ�״̬��: {code}");
                break;
        }
    }

}
