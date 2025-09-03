using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WindCtr : MonoBehaviour
{
    public BoxCollider2D windArea;
    public AreaEffector2D windAreaEffector;
    [Header("风力设置")]
    [Tooltip("风力大小")]
    public float windForce = 20f;
    [Header("风力检测层级")]
    [Tooltip("风力检测的LayerMask，需与AreaEffector2D的Force Target Layer一致")]
    public LayerMask windLayerMask;

    public ParticleSystem windParticle;

    [Header("风力范围设置")]
    [Tooltip("风力作用范围宽度")]
    public float windWidth = 5f;
    [Tooltip("风力作用范围高度")]
    public float windHeight = 2f;

    private BoxCollider2D windCollider;

    void OnValidate()
    {
        // 保证windRb存在
        if (windArea == null)
            windArea = GetComponent<BoxCollider2D>();
        if (windAreaEffector == null)
            windAreaEffector = GetComponent<AreaEffector2D>();
        if (windParticle == null)
            windParticle = GetComponentInChildren<ParticleSystem>();
        if (windCollider == null)
            windCollider = GetComponent<BoxCollider2D>();

        // windWidth和windHeight与BoxCollider2D的范围和位置保持一致
        if (windCollider != null)
        {
            windWidth = windCollider.size.x;
            windHeight = windCollider.size.y;
        }

        // 设置AreaEffector2D的力大小
        if (windAreaEffector != null)
        {
            windAreaEffector.forceMagnitude = windForce;
            windAreaEffector.forceAngle = transform.eulerAngles.z;
            windAreaEffector.colliderMask = windLayerMask;
        }

        // 设置BoxCollider2D的范围
        if (windCollider != null)
        {
            windCollider.size = new Vector2(windWidth, windHeight);
            windCollider.isTrigger = true;
        }

        // 设置ParticleSystem的Shape范围和旋转
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
