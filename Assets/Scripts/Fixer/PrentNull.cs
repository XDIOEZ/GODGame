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
        // �ȴ�һ֡
        yield return null;

        // ���������
        transform.SetParent(null);
    }
}
