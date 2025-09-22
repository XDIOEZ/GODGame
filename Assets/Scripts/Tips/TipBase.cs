using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipBase : MonoBehaviour
{
    public GameObject tip;
    public bool tipSwitch;

    private void Start()
    {
        tipSwitch = TipsManger.Instance.allKey;
        OnOrOff();
    }

    private void OnOrOff()
    {
        if (tipSwitch)
        {
            tip.SetActive(true);
        }
        else tip.SetActive(false);
    }
}
