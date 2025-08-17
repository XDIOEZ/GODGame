using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PersistentDataUtil_Json;

public class PersistentTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [Button("≤‚ ‘_–¥»Î")]
    public static void Test()
    {
        PlayerData data = new PlayerData();
        data.name = "≤‚ ‘";
        PersistentDataUtil_Json.Save("PlayerData", data);

    }

    [Button("≤‚ ‘_∂¡»°")]
    public static void Test_Load()
    {
        PlayerData data = new PlayerData();
        PersistentDataUtil_Json.Load("PlayerData", ref data);
        Debug.Log(data.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
