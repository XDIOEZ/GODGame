using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WindCtr : MonoBehaviour
{
    public BoxCollider2D windArea;
    public AreaEffector2D windAreaEffector;
    [Header("��������")]
    [Tooltip("������С")]
    public float windForce = 20f;
    [Header("�������㼶")]
    [Tooltip("��������LayerMask������AreaEffector2D��Force Target Layerһ��")]
    public LayerMask windLayerMask;

    public ParticleSystem windParticle;

    [Header("������Χ����")]
    [Tooltip("�������÷�Χ���")]
    public float windWidth = 5f;
    [Tooltip("�������÷�Χ�߶�")]
    public float windHeight = 2f;

    private BoxCollider2D windCollider;

    void OnValidate()
    {
        // ��֤windRb����
        if (windArea == null)
            windArea = GetComponent<BoxCollider2D>();
        if (windAreaEffector == null)
            windAreaEffector = GetComponent<AreaEffector2D>();
        if (windParticle == null)
            windParticle = GetComponentInChildren<ParticleSystem>();
        if (windCollider == null)
            windCollider = GetComponent<BoxCollider2D>();

        // windWidth��windHeight��BoxCollider2D�ķ�Χ��λ�ñ���һ��
        if (windCollider != null)
        {
            windWidth = windCollider.size.x;
            windHeight = windCollider.size.y;
        }

        // ����AreaEffector2D������С
        if (windAreaEffector != null)
        {
            windAreaEffector.forceMagnitude = windForce;
            windAreaEffector.forceAngle = transform.eulerAngles.z;
            windAreaEffector.colliderMask = windLayerMask;
        }

        // ����BoxCollider2D�ķ�Χ
        if (windCollider != null)
        {
            windCollider.size = new Vector2(windWidth, windHeight);
            windCollider.isTrigger = true;
        }

        // ����ParticleSystem��Shape��Χ����ת
        if (windParticle != null)
        {
            var shape = windParticle.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(windWidth, windHeight, 1f);
            windParticle.transform.rotation = transform.rotation;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.3f, 0.7f, 1f, 0.3f);
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, new Vector3(windWidth, windHeight, 0.1f));
        Gizmos.matrix = oldMatrix;
    }
}
