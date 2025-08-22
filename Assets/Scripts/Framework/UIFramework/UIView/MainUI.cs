using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : BasePanel
{
    public Button secendBtn; 
    public override void OnEnter()
    {
        Debug.Log("������");
        secendBtn.onClick.AddListener(ClickSecendBtn);
    }

    public override void OnExit()
    {
        base.OnExit();//��Ҫ�̳�ԭ��Ĺر�����ҳ��
        Debug.Log("�ټ����");
        Destroy(this); 
    }

    private void ClickSecendBtn()
    {
        UIManager.Instance.Show(UIPanelType.SecendView);
    }
}
