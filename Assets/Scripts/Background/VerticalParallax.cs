using UnityEngine;

public class VerticalParallax : MonoBehaviour
{
    [Header("视差设置")]
    public bool useCustomFactor = false;           // ✅ 是否使用自定义因子
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;            // 0=固定不动，1=跟随相机

    private Transform cameraTransform;
    private float startY;
    private float camStartY;

    void Start()
    {
        // 自动获取主相机
        cameraTransform = Camera.main?.transform;

        if (cameraTransform != null)
        {
            startY = transform.position.y;
            camStartY = cameraTransform.position.y;
        }
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        float camDeltaY = cameraTransform.position.y - camStartY;
        transform.position = new Vector3(
            transform.position.x,
            startY + camDeltaY * parallaxFactor,
            transform.position.z
        );
    }
}
