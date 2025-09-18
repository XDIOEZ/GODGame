using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 音效种类
/// </summary>
public enum AudioType
{
    BGM,
    Up,
    Full,
    Btn,
    GameWin,
    GameLose
}
public class AudioManager : MonoBehaviour
{
    [Header("音效数据")]
    public List<AudioDate> audioDate;

    private Dictionary<AudioType, AudioDate> audioDict = new Dictionary<AudioType, AudioDate>();

    [Header("按钮音效设置")]
    public AudioType buttonSoundType = AudioType.Btn; // 按钮点击音效类型
    public List<Button> buttonsWithSound; // 需要在外部拖入的按钮列表

    [Serializable]
    public struct AudioDate
    {
        public AudioType audioType;
        public AudioClip audioClip;
        public AudioSource audioSource;
    }

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioDict();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 确保字典已初始化
        if (audioDict == null || audioDict.Count == 0)
        {
            InitializeAudioDict();
        }

        // 为所有按钮添加点击音效
        SetupButtonSounds();

        // 播放BGM
        if (audioDate.Count > 0 && audioDate[0].audioSource != null)
        {
            audioDate[0].audioSource.Play();
        }
    }

    // 初始化音频字典
    private void InitializeAudioDict()
    {
        audioDict = new Dictionary<AudioType, AudioDate>();
        foreach (var audio in audioDate)
        {
            if (audio.audioSource != null)
            {
                audio.audioSource.clip = audio.audioClip;
                // 根据音效类型设置不同的循环属性
                if (audio.audioType == AudioType.BGM)
                {
                    audio.audioSource.loop = true; // 只有BGM循环
                }
                else
                {
                    audio.audioSource.loop = false; // 其他音效不循环
                }
                audio.audioSource.volume = 0.7f;
            }
            audioDict[audio.audioType] = audio;
            Debug.Log($"已注册音效类型: {audio.audioType}, Clip: {audio.audioClip?.name}, Loop: {audio.audioSource?.loop}");
        }
    }

    // 设置按钮点击音效
    private void SetupButtonSounds()
    {
        if (buttonsWithSound == null) return;

        foreach (Button button in buttonsWithSound)
        {
            if (button != null)
            {
                // 移除可能已存在的监听器，避免重复添加
                button.onClick.RemoveAllListeners();
                // 添加点击音效监听器
                button.onClick.AddListener(() => PlayButtonSound());

                Debug.Log($"已为按钮 {button.name} 添加点击音效");
            }
        }
    }

    // 播放按钮点击音效
    private void PlayButtonSound()
    {
        PlayAudio(buttonSoundType);
        Debug.Log("播放按钮点击音效");
    }

    // 手动添加按钮到音效列表
    public void AddButtonWithSound(Button button)
    {
        if (button != null && !buttonsWithSound.Contains(button))
        {
            buttonsWithSound.Add(button);
            // 立即为新增按钮添加点击音效
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => PlayButtonSound());

            Debug.Log($"已添加按钮 {button.name} 到音效列表");
        }
    }

    // 移除按钮的音效
    public void RemoveButtonSound(Button button)
    {
        if (button != null && buttonsWithSound.Contains(button))
        {
            buttonsWithSound.Remove(button);
            // 移除点击监听器（注意：这会移除所有监听器）
            button.onClick.RemoveAllListeners();

            Debug.Log($"已移除按钮 {button.name} 的音效");
        }
    }

    private void OnEnable()
    {
        EventController.OnPlayAudio += PlayerAudio;
        EventController.OnStopAudio += StopAudio;
        EventController.OnSetAudioVolume += SetAudioVolume;
    }

    private void OnDisable()
    {
        EventController.OnPlayAudio -= PlayerAudio;
        EventController.OnStopAudio -= StopAudio;
        EventController.OnSetAudioVolume -= SetAudioVolume;
    }

    // 音量控制方法
    private void SetAudioVolume(AudioType audioType, float volume)
    {
        if (audioDict.TryGetValue(audioType, out AudioDate audioDate))
        {
            if (audioDate.audioSource != null)
            {
                audioDate.audioSource.volume = Mathf.Clamp01(volume);
            }
        }
    }

    private void PlayerAudio(AudioType audioType)
    {
        PlayAudio(audioType);
    }

    // 公开的播放音效方法
    public void PlayAudio(AudioType audioType)
    {
        if (audioDict.TryGetValue(audioType, out AudioDate audioDate))
        {
            if (audioDate.audioSource != null)
            {
                audioDate.audioSource.Play();
                Debug.Log("播放音效: " + audioType);
            }
        }
    }

    public void StopAudio(AudioType audioType)
    {
        if (audioDict.TryGetValue(audioType, out AudioDate audioDate))
        {
            if (audioDate.audioSource != null && audioDate.audioSource.isPlaying)
            {
                audioDate.audioSource.Stop();
                Debug.Log("停止音效: " + audioType);
            }
        }
    }

    // 添加静态方法方便调用
    public static void StopAudioStatic(AudioType audioType)
    {
        if (Instance != null)
        {
            Instance.StopAudio(audioType);
        }
    }

    // 静态方法播放按钮音效
    public static void PlayButtonSoundStatic()
    {
        if (Instance != null)
        {
            Instance.PlayButtonSound();
        }
    }

    // 在场景重新加载时重新设置按钮音效
    public void OnLevelWasLoaded(int level)
    {
        // 延迟一帧执行，确保所有按钮都已初始化
        Invoke("SetupButtonSounds", 0.1f);
    }
}