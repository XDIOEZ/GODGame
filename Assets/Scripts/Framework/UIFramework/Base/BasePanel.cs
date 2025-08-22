using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BasePanel : MonoBehaviour {
    /// <summary>
    /// 界面显示
    /// </summary>
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPause()
    {

    }

    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume()
    {

    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面关闭离栈
    /// </summary>
    public virtual void OnExit()
    {
        Destroy(this.gameObject);
        UIManager.Instance.ClearPanelCache(this);
    }

    public void DestroySelf()
    {
        UIManager.Instance.destroy();
    }
}
