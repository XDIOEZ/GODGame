using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public AudioType buttonSoundType = AudioType.Btn;
    public string buttonTag = "SoundButton"; // 指定标签名称

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

        // 自动绑定带标签的按钮
        AutoBindTaggedButtons();


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

     // 自动绑定带指定标签的按钮
    private void AutoBindTaggedButtons()
    {
        // 方法1：通过标签查找
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(buttonTag);
        foreach (GameObject obj in taggedObjects)
        {
            Button button = obj.GetComponent<Button>();
            if (button != null)
            {
                BindButtonSound(button);
            }
        }

        Debug.Log($"已自动绑定 {taggedObjects.Length} 个带标签按钮的音效");
    }

    // 为单个按钮绑定音效
    private void BindButtonSound(Button button)
    {
        if (button == null) return;

        // 移除已有监听器（避免重复）
        button.onClick.RemoveListener(PlayButtonSound);
        button.onClick.AddListener(PlayButtonSound);
    }

    // 场景加载完成后重新绑定按钮
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Invoke(nameof(AutoBindTaggedButtons), 0.1f);
    }

    

   

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventController.OnPlayAudio += PlayerAudio;
       
        EventController.OnPlayAudio += PlayerAudio;
        EventController.OnStopAudio += StopAudio;
        EventController.OnSetAudioVolume += SetAudioVolume;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventController.OnPlayAudio -= PlayerAudio;
        
        EventController.OnPlayAudio -= PlayerAudio;
        EventController.OnStopAudio -= StopAudio;
        EventController.OnSetAudioVolume -= SetAudioVolume;
    }
    // 播放按钮点击音效
    private void PlayButtonSound()
    {
        PlayAudio(buttonSoundType);
    }

    // 手动绑定指定按钮（可用于动态创建的按钮）
    public void BindSpecificButton(Button button)
    {
        if (button != null)
        {
            BindButtonSound(button);
            Debug.Log($"已为按钮 {button.name} 绑定音效");
        }
    }

    // 手动解绑按钮
    public void UnbindButton(Button button)
    {
        if (button != null)
        {
            button.onClick.RemoveListener(PlayButtonSound);
            Debug.Log($"已移除按钮 {button.name} 的音效绑定");
        }
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