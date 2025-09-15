using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    public static UnityAction<AudioType> OnPlayAudio;
    public static void RaiseOnPlayAudio(AudioType type)
    {
               OnPlayAudio?.Invoke(type);
    }
}
