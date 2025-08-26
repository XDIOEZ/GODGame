using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBase : MonoBehaviour
{
    public void Start()
    {
        GetComponentInParent<InteractReciver>().onInteractEvent_Start += Action;
    }
    public virtual void Action(Interacter interacter)
    {
        Debug.Log("InteractBase Action");
    }
}
