using System.Collections;
using UnityEngine;

public class ToGreen : InteractBase
{
    public SpriteRenderer spriteRenderer;
    public Color targetColor = Color.green; // 默认目标颜色为绿色
    public float duration = 1f; // 渐变持续时间

    private Coroutine colorCoroutine;


    public override void Action(Interacter interacter)
    {
        // 如果上一次渐变还在进行，先停止
        if (colorCoroutine != null)
            StopCoroutine(colorCoroutine);

        colorCoroutine = StartCoroutine(ChangeColorCoroutine(targetColor, duration));
    }

    private IEnumerator ChangeColorCoroutine(Color target, float duration)
    {
        Color startColor = spriteRenderer.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startColor, target, elapsed / duration);
            yield return null;
        }

        // 保证最终颜色精确
        spriteRenderer.color = target;
    }
}
