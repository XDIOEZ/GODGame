using UnityEngine;

[ExecuteAlways]
public class ParallaxGroupManager : MonoBehaviour
{
    [Header("自动分配参数")]
    [Range(0f, 1f)] public float minFactor = 0.1f; // 最远背景
    [Range(0f, 1f)] public float maxFactor = 1f;   // 最近背景

    private void Start()
    {
        ApplyParallaxFactors();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ApplyParallaxFactors();
    }
#endif

    public void ApplyParallaxFactors()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            VerticalParallax vp = child.GetComponent<VerticalParallax>();
            if (vp == null) vp = child.gameObject.AddComponent<VerticalParallax>();

            if (vp.useCustomFactor)
                continue; // ✅ 跳过自定义的，不改 parallaxFactor


            // 修正：索引小的（靠前的子对象） => 因子大
            float t = 1f - (float)i / (childCount - 1); // 反转
            vp.parallaxFactor = Mathf.Lerp(minFactor, maxFactor, t);
        }
    }
}
