using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ParameterFuel : ParameterBase
{
    [Header("当前最大燃料")]
    public float maxFuel = 100;
    [Header("当前剩余燃料")]
    public float fuel = 10;
    [Header("当前燃料燃烧系数")]
    [SerializeField]
    private int fuelCoefficient = 1;
    [Header("当前燃料恢复系数")]
    [SerializeField]
    private int fuelReCoverNum = 1;
    [Tooltip("要检测的目标图层（如Player层）")]
    public LayerMask targetLayer;        // 统一使用Layer检测（已修正）


    private float maxRange;
    private float minRange;




    private void Start()
    {
        if (fuel > maxFuel)
        {
            fuel = maxFuel;
        }

      //  maxRange = (mianEngineDate.thrustForce/100) * (maxFuel/ fuelCoefficient) * mianEngineDate.fallingspeed.Fallingspeed;
    }
    public int FuelCoefficient =>fuelCoefficient;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 检查碰撞物体是否在目标图层中
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            Recover();
        }
    }

    private void Recover()
    {
        if (fuel <= maxFuel)
        {
            fuel += Time.deltaTime * fuelReCoverNum;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxRange); // 估算最大范围圈
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minRange); // 估算最小距离圈
    }

}
