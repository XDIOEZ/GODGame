using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParameterFuel : ParameterBase
{
    [Header("��ǰ���ȼ��")]
    public float maxFuel = 100;
    [Header("��ǰʣ��ȼ��")]
    public float fuel = 10;
    [Header("��ǰȼ��ȼ��ϵ��")]
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
