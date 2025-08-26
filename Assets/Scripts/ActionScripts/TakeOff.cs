using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOff : InteractBase
{
    public float Power = 1.0f;
    public Vector2   Direction = Vector2.up;

    public override void Action(Interacter interacter)
    {
        interacter.owner.GetComponent<Rigidbody2D>().AddForce(Direction * Power);
    }
}
