using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : InteractBase
{
    public static UltEvent onSceneChange = new UltEvent();
    public string sceneName;
    public override void Action(Interacter interacter)
    {
        if (sceneName == "")
        {
            onSceneChange.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            onSceneChange.Invoke();
            SceneManager.LoadScene(sceneName);
        }
     
    }
}
