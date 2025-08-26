using System.Collections;
using UnityEngine;

public class ToGreen : InteractBase
{
    public SpriteRenderer spriteRenderer;
    public Color targetColor = Color.green; // Ĭ��Ŀ����ɫΪ��ɫ
    public float duration = 1f; // �������ʱ��

    private Coroutine colorCoroutine;


    public override void Action(Interacter interacter)
    {
        // �����һ�ν��仹�ڽ��У���ֹͣ
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

        // ��֤������ɫ��ȷ
        spriteRenderer.color = target;
    }
}
