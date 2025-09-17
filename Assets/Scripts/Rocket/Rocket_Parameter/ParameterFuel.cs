using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ParameterFuel : ParameterBase
{
    [Header("��ǰ���ȼ��")]
    public float maxFuel = 100;
    [Header("��ǰʣ��ȼ��")]
    public float fuel = 10;
    [Header("��ǰȼ��ȼ��ϵ��")]
    [SerializeField]
    private int fuelCoefficient = 1;
    [Header("��ǰȼ�ϻָ�ϵ��")]
    [SerializeField]
    private int fuelReCoverNum = 1;
    [Tooltip("Ҫ����Ŀ��ͼ�㣨��Player�㣩")]
    public LayerMask targetLayer;        // ͳһʹ��Layer��⣨��������


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
        // �����ײ�����Ƿ���Ŀ��ͼ����
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
        Gizmos.DrawWireSphere(transform.position, maxRange); // �������ΧȦ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minRange); // ������С����Ȧ
    }

}
