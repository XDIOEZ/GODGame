using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TapSDK.Compliance;
using UnityEngine;
using UnityEngine.UI;

public class TimerCount : MonoBehaviour
{
    [Header("设置")]
    public float updateInterval = 1.0f; // 更新间隔(秒)
    public float serverCheckInterval = 300.0f; // 服务器检查间隔(秒)
    public int warningThreshold = 300; // 警告阈值(5分钟)

    private int _remainingTime = 0; // 当前剩余时间(秒)
    private float _lastServerCheckTime = 0; // 上次服务器检查时间
    private bool _isTimeWarningActive = false;

    // 退出游戏事件
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
        // 启动实时更新协程
        StartCoroutine(RealTimeUpdate());
        // 立即获取初始时间
        _ = FetchInitialTimeAsync();
    }

    private async Task FetchInitialTimeAsync()
    {
        try
        {
            int ageRange = await TapTapCompliance.GetAgeRange();
            _remainingTime = await TapTapCompliance.GetRemainingTime();
            Debug.Log($"初始剩余时间: {FormatTime(_remainingTime)}");
            Debug.Log($"玩家当前年龄: {ageRange}");
        }
        catch (Exception e)
        {
            Debug.LogError($"获取初始时间失败: {e.Message}");
        }
    }

    private IEnumerator RealTimeUpdate()
    {
        while (true)
        {
            // 更新本地计时
            if (_remainingTime > 0)
            {
                _remainingTime--;
                Debug.Log($"剩余时间: {FormatTime(_remainingTime)}");
            }

            // 检查是否需要服务器同步
            if (Time.time - _lastServerCheckTime > serverCheckInterval)
            {
                _ = SyncWithServerTime();
            }

            // 检查是否需要警告或退出
            CheckTimeStatus();

            yield return new WaitForSeconds(updateInterval);
        }
    }

    private async Task SyncWithServerTime()
    {
        try
        {
            int serverTime = await TapTapCompliance.GetRemainingTime();

            // 如果服务器时间与本地时间差异较大，进行校准
            if (Math.Abs(serverTime - _remainingTime) > 10)
            {
                _remainingTime = serverTime;
                Debug.Log($"时间已校准: {FormatTime(_remainingTime)}");
            }

            _lastServerCheckTime = Time.time;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"时间同步失败: {e.Message}");
        }
    }

    private void CheckTimeStatus()
    {
        // 当剩余时间低于阈值且未激活警告时
        if (_remainingTime <= warningThreshold && !_isTimeWarningActive)
        {
            ShowTimeWarning();
        }

        // 当时间耗尽时强制退出游戏
        if (_remainingTime <= 0)
        {
            ForceQuitGame();
        }
    }

    private void ShowTimeWarning()
    {
        _isTimeWarningActive = true;
        Debug.LogWarning($"警告: 剩余游戏时间不足 {warningThreshold / 60} 分钟!");

        // 这里可以添加其他警告逻辑，如播放声音等
    }

    private void ForceQuitGame()
    {
        Debug.Log("游戏时间已耗尽，强制退出游戏");

        // 触发退出事件
        OnForceQuitGame?.Invoke();

        // 实际退出游戏
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

    //// 当游戏暂停时停止计时
    //private void OnApplicationPause(bool pauseStatus)
    //{
    //    if (pauseStatus)
    //    {
    //        StopAllCoroutines();
    //    }
    //    else
    //    {
    //        StartCoroutine(RealTimeUpdate());
    //        _ = SyncWithServerTime(); // 恢复时立即同步
    //    }
    //}
}
