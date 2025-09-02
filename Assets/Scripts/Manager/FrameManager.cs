using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameManager : MonoBehaviour
{
    public int targetFrameRate = -1;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
