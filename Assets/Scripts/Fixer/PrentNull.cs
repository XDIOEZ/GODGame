using System.Collections;
using UnityEngine;

public class PrentNull : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DetachParentNextFrame());
    }

    private IEnumerator DetachParentNextFrame()
    {
        // 等待一帧
        yield return null;

        // 解除父物体
        transform.SetParent(null);
    }
}
