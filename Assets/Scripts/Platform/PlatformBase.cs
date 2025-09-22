using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum PlatFormType
{
    Normal,
    Ice,
    Cloud,
    Move,
}


public class PlatformBase : MonoBehaviour
{
    [SerializeField]
    private bool Canmove=false;
    [SerializeField]
    private PlatFormType Type;

    public PlatFormTip Tip;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Tip = GetComponent<PlatFormTip>();


        Tip.tip = transform.Find("Tip").gameObject;
    }
}
