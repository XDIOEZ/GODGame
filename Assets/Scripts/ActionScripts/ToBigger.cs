using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Action(Interacter interacter)
    {
        interacter.owner.transform.localScale *= 1.1f; // ·Å´ó1.1±¶
    }
}
