using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ParameterType
{
    Fallingspeed,
    Fuel,
    Materials,
    ShipDurability,
    Thrust,
    Steeringspeed
}
public class ParameterBase : MonoBehaviour
{
    public int ID;

    public ParameterType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
