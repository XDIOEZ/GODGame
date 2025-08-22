using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class secendUI : BasePanel
{

    public Button CloseBtn;
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("二级面板打开");
        CloseBtn.onClick.AddListener(CloseSelf);
    }

    public override void OnExit()
    {
        base.OnExit();

    }
    private void CloseSelf()
    {
        this.DestroySelf();  
    }

}
