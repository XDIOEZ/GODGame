using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class StartSceneUI : BasePanel
{
    public Button closeButton;
    public Button openNewView;
    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.AddListener(CloseSelf);
        openNewView.onClick.AddListener(OpenNewView);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenNewView()
    {
        UIManager.Instance.Show(UIPanelType.SecendView);
    }

    private void CloseSelf()
    {
        this.DestroySelf();
    }

}
