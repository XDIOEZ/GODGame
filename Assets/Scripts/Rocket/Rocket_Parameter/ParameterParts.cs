using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterParts : ParameterBase
{
    [Header("最大零部件")]
    [SerializeField]
    private int maxparts;

    [Header("当前零部件")]
    [SerializeField]
    private int parts;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        EventManager.Instance.On<ParameterPartsTouchEvent>(ChangeParts);
    }

    private void OnDisable()
    {
        EventManager.Instance.Off<ParameterPartsTouchEvent>(ChangeParts);
    }

    private void ChangeParts(ParameterPartsTouchEvent evt)
    {
        parts++;
    }
}
