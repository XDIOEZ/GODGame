using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TapSDK.Compliance;
using UnityEngine;
using UnityEngine.UI;

public class TimerCount : MonoBehaviour
{
    [Header("����")]
    public float updateInterval = 1.0f; // ���¼��(��)
    public float serverCheckInterval = 300.0f; // �����������(��)
    public int warningThreshold = 300; // ������ֵ(5����)

    private int _remainingTime = 0; // ��ǰʣ��ʱ��(��)
    private float _lastServerCheckTime = 0; // �ϴη��������ʱ��
    private bool _isTimeWarningActive = false;

    // �˳���Ϸ�¼�
    public event Action OnForceQuitGame;

    private void Start()
    {
       // Invoke("DelayedStart", 1f);
    }

    private void Update()
    {

    }

    private void DelayedStart()
    {
        // ����ʵʱ����Э��
        StartCoroutine(RealTimeUpdate());
        // ������ȡ��ʼʱ��
        _ = FetchInitialTimeAsync();
    }

    private async Task FetchInitialTimeAsync()
    {
        try
        {
            int ageRange = await TapTapCompliance.GetAgeRange();
            _remainingTime = await TapTapCompliance.GetRemainingTime();
            Debug.Log($"��ʼʣ��ʱ��: {FormatTime(_remainingTime)}");
            Debug.Log($"��ҵ�ǰ����: {ageRange}");
        }
        catch (Exception e)
        {
            Debug.LogError($"��ȡ��ʼʱ��ʧ��: {e.Message}");
        }
    }

    private IEnumerator RealTimeUpdate()
    {
        while (true)
        {
            // ���±��ؼ�ʱ
            if (_remainingTime > 0)
            {
                _remainingTime--;
                Debug.Log($"ʣ��ʱ��: {FormatTime(_remainingTime)}");
            }

            // ����Ƿ���Ҫ������ͬ��
            if (Time.time - _lastServerCheckTime > serverCheckInterval)
            {
                _ = SyncWithServerTime();
            }

            // ����Ƿ���Ҫ������˳�
            CheckTimeStatus();

            yield return new WaitForSeconds(updateInterval);
        }
    }

    private async Task SyncWithServerTime()
    {
        try
        {
            int serverTime = await TapTapCompliance.GetRemainingTime();

            // ���������ʱ���뱾��ʱ�����ϴ󣬽���У׼
            if (Math.Abs(serverTime - _remainingTime) > 10)
            {
                _remainingTime = serverTime;
                Debug.Log($"ʱ����У׼: {FormatTime(_remainingTime)}");
            }

            _lastServerCheckTime = Time.time;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"ʱ��ͬ��ʧ��: {e.Message}");
        }
    }

    private void CheckTimeStatus()
    {
        // ��ʣ��ʱ�������ֵ��δ�����ʱ
        if (_remainingTime <= warningThreshold && !_isTimeWarningActive)
        {
            ShowTimeWarning();
        }

        // ��ʱ��ľ�ʱǿ���˳���Ϸ
        if (_remainingTime <= 0)
        {
            ForceQuitGame();
        }
    }

    private void ShowTimeWarning()
    {
        _isTimeWarningActive = true;
        Debug.LogWarning($"����: ʣ����Ϸʱ�䲻�� {warningThreshold / 60} ����!");

        // �������������������߼����粥��������
    }

    private void ForceQuitGame()
    {
        Debug.Log("��Ϸʱ���Ѻľ���ǿ���˳���Ϸ");

        // �����˳��¼�
        OnForceQuitGame?.Invoke();

        // ʵ���˳���Ϸ
        QuitApplication();
    }

    private void QuitApplication()
    {
        //#if UNITY_EDITOR
        //UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //Application.Quit();
        //#endif
    }

    private string FormatTime(int seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }

    //// ����Ϸ��ͣʱֹͣ��ʱ
    //private void OnApplicationPause(bool pauseStatus)
    //{
    //    if (pauseStatus)
    //    {
    //        StopAllCoroutines();
    //    }
    //    else
    //    {
    //        StartCoroutine(RealTimeUpdate());
    //        _ = SyncWithServerTime(); // �ָ�ʱ����ͬ��
    //    }
    //}
}
