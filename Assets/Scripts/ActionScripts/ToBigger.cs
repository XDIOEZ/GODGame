using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBigger : InteractBase
{

    public override void Action(Interacter interacter)
    {
        interacter.owner.transform.localScale *= 2.1f; // �Ŵ�1.1��
    }
}
