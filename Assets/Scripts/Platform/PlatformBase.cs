using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum PlatFormType
{
    Normal,
    Ice,
    Cloud,
    Move,
}


public class PlatformBase : MonoBehaviour
{
    [SerializeField]
    private bool Canmove=false;
    [SerializeField]
    private PlatFormType Type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
