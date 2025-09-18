using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    public static UnityAction<AudioType> OnPlayAudio;
    public static UnityAction<AudioType> OnStopAudio;
    public static UnityAction<AudioType, float> OnSetAudioVolume; // 音量控制事件
    public static void RaiseOnPlayAudio(AudioType type)
    {
               OnPlayAudio?.Invoke(type);
    }
    public static void RaiseOnStopAudio(AudioType type)
    {
        OnStopAudio?.Invoke(type);
    }
    public static void RaiseOnSetAudioVolume(AudioType type, float volume)
    {
        OnSetAudioVolume?.Invoke(type, volume);
    }
}
