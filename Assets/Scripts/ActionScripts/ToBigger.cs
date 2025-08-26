using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBigger : InteractBase
{

    public override void Action(Interacter interacter)
    {
        interacter.owner.transform.localScale *= 1.1f; // ·Å´ó1.1±¶
    }
}
