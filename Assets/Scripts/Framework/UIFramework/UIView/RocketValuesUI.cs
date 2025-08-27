
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketValuesUI : BasePanel
{

    public List<Text> values;
    public Button backButten;

    public override void OnEnter()
    {
        base.OnEnter();
        Time.timeScale = 0f; // ‘›Õ£”Œœ∑
        backButten.onClick.AddListener(OnExit);
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void OnExit()
    {
        base.OnExit();
        Time.timeScale = 1f; // ª÷∏¥”Œœ∑
    }

}
