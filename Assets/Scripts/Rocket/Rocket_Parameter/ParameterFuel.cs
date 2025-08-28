using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParameterFuel : ParameterBase
{
    [Header("当前最大燃料")]
    public float maxFuel = 100;
    [Header("当前剩余燃料")]
    public float fuel = 10;
    [Header("当前燃料燃烧系数")]
    private int fuelCoefficient = 1;

    private void Start()
    {
        if (fuel > maxFuel)
        {
            fuel = maxFuel;
        }
    }
    public int FuelCoefficient =>fuelCoefficient;
}
