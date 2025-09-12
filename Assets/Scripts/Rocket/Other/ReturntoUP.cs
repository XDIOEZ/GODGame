using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReturntoUP : MonoBehaviour
{
    [Header("��Ҫ�󶨵ķɴ���ǩ��tag����")]
    [SerializeField]
    private string playerName;

    [Header("Ҫ����Ŀ��ͼ�㣨��Ground�㣩")]
    public LayerMask targetLayer;        // ͳһʹ��Layer��⣨��������

    [Header("�����������ʱ��")]
    [SerializeField]
    public float retrunTimer=1f;        // ͳһʹ��Layer��⣨��������


    [Header("����������н�")]
    [Tooltip("ֻ��0��180��֮��")]
    [SerializeField]
    private float retrunAngle = 0;        // ͳһʹ��Layer��⣨��������

    [Header("ȼ�����")]
    public ParameterFuel fuel;        // ͳһʹ��Layer��⣨��������

    [Header("ƽ����ת����")]
    public float smoothRotationTime = 0.5f; // ƽ����ת����ʱ��
    private bool isSmoothingRotation = false; // ƽ����ת״̬��־
    private float currentReturnTimer = 0; // ƽ����ת��ʱ��

    [SerializeField]
    private float currentRetrunTimer = 0f;        // ͳһʹ��Layer��⣨��������


    [SerializeField]
    private Transform PlayerTransform;
    // Start is called before the first frame update
    void Start()
    {
        // �Զ��ҵ���ң����û�ֶ��ϣ����ݱ�ǩ�ң���ұ�ǩ��Ϊ"Player"��
        if (PlayerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerName);
            if (player != null)
                PlayerTransform = player.transform;
            else
                Debug.LogError("δ�ҵ���ң����������ñ�ǩ'Player'�����ֶ���ֵplayerTarget");
        }
    }

    private void Update()
    {
        if (fuel.fuel <= 0)
        {
            ReturnShopLowBool();
        }
        
    }

    private void FixedUpdate()
    {
        //ReturnShopLow();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            CheckTimerReturn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentRetrunTimer = 0;
    }


    private void CheckTimerReturn()
    {
        //Debug.Log(PlayerTransform.eulerAngles.z);

        if (Mathf.Abs(PlayerTransform.eulerAngles.z) > retrunAngle)
        {
            //Debug.Log("�Ƿ񴥷������Ƕ�");
            currentRetrunTimer += Time.deltaTime;
        if (currentRetrunTimer >= retrunTimer)
        {
                ReturnShopRight();
        }
    }
        else { currentRetrunTimer = 0; }
    }

    private void ReturnShopRight()
    {
        // ����z����תΪ0������x��y����ת���䣩
        Vector3 currentEuler = PlayerTransform.eulerAngles;
        currentEuler.z = 0; // ֻ����z��
        PlayerTransform.eulerAngles = currentEuler;
        currentRetrunTimer = 0;
    }

    // ƽ�����ɽ�Z����ת����
    private void ReturnShopLowBool()
    {
        isSmoothingRotation = true; // ����ƽ����ת��־
        currentReturnTimer = 0; // ���ü�ʱ��
    }
    private void ReturnShopLow()
    {
        // �����Ҫƽ����ת����Update���𲽹���
        if (isSmoothingRotation)
        {
            currentReturnTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(currentReturnTimer / smoothRotationTime);

            // ��ȡ��ǰ��ת��ֻƽ��Z��
            Vector3 currentEuler = PlayerTransform.eulerAngles;
            // ʹ��Lerp����ƽ�����ɣ��ӵ�ǰZֵ���ɵ�0
            currentEuler.z = Mathf.LerpAngle(currentEuler.z, 0, progress);
            PlayerTransform.eulerAngles = currentEuler;

            // ����ת��ɺ�ֹͣƽ��
            if (progress >= 1f)
            {
                isSmoothingRotation = false;
                currentEuler.z = 0; // ȷ�����վ�ȷ��0
                PlayerTransform.eulerAngles = currentEuler;
            }
        }
    }

    
}
