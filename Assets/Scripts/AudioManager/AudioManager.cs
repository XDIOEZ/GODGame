using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //确保只有一个实例存在
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
       
    }
    private void OnEnable()
    {
        audioDate[0].audioSource.Play();
        EventController.OnPlayAudio+=PlayerAudio;
    }
    private void OnDisable()
    {
        EventController.OnPlayAudio-=PlayerAudio;
    }
    private void OnServerInitialized()
    {
        audioDict = new Dictionary<AudioType, AudioDate>();
        foreach (var audio in audioDate)
        {
            if(audio.audioSource !=null)
            {
                audio.audioSource.clip = audio.audioClip;
            }
            audioDict[audio.audioType] = audio;
        }
    }
    private void PlayerAudio(AudioType audioType)
    {
        if (audioDict.TryGetValue(audioType, out AudioDate audioDate))
        {
            audioDict[audioType].audioSource.Play();
            Debug.Log("播放音效" + audioType);
        }
    }
}
