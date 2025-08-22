using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOff : MonoBehaviour
{
    public float Power = 1.0f;
    public Vector2   Direction = Vector2.up;

    public void Action(Interacter interacter)
    {
        interacter.owner.GetComponent<Rigidbody2D>().AddForce(Direction * Power);
    }
}
