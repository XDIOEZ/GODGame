using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParameterFuel : ParameterBase
{
    [Header("��ǰʣ��ȼ��")]
    public float fuel = 10;
    [Header("��ǰȼ��ȼ��ϵ��")]
    private int fuelCoefficient = 1;


    public int FuelCoefficient =>fuelCoefficient;
}
