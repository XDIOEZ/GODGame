using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : BasePanel
{
    public Button secendBtn; 
    public override void OnEnter()
    {
        Debug.Log("你好面板");
        secendBtn.onClick.AddListener(ClickSecendBtn);
    }

    public override void OnExit()
    {
        base.OnExit();//需要继承原版的关闭自身页面
        Debug.Log("再见面板");
        Destroy(this); 
    }

    private void ClickSecendBtn()
    {
        UIManager.Instance.Show(UIPanelType.SecendView);
    }
}
