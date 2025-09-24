using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameEndText : MonoBehaviour
{
    [Header("UI���")]
    public Text deathCountText;
    public RectTransform textContainer;
    public RectTransform canvasRect;
    
    [Header("��������")]
    [Tooltip("����ʱ��(��)")]
    public float scrollDuration = 20f;
    
[Header("��������")]
[TextArea(10, 20)]
public string endText = @"����Ϊ֤���ǳ�����

������������Ƭ����
��д�������Լ��Ĵ���

�����εĵ���������
��֤������������֮��

����С�����
���Ի͵��յ�
ÿһ����ֵ������

Ӣ�ۣ������;���ǳ���
�������ѵִ�˰�

Ը�������
����������Զ��ҫ

ֱ����һ��ð�յĿ�ʼ
Ը����ķ�
��Զ����������...";
    private bool isScrolling = false;

    private void Start()
    {

        // ��ȡCanvas RectTransform�����δָ�����Զ���ȡ��
        if (canvasRect == null)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                canvasRect = canvas.GetComponent<RectTransform>();
            }
        }

        // ��ȡ���������������ʾ
        int deathCount = Checkpoint.GetPlayerDeathCount();

        // ��ȡ�������Ϸʱ��
        string playTime = PlayerPlayTimeManager.GetFormattedPlayTime();

        // �����������ݣ�����������������Ϸʱ��
        if (deathCountText != null)
        {
            deathCountText.text = $"��Ϸ����\n\n�����������: {deathCount}\n����ʱ��: {playTime}\n\n{endText}";
        }

        // ������ʼλ������Ļ�ײ�����
        if (textContainer != null && canvasRect != null)
        {
            Vector3 startPosition = textContainer.anchoredPosition;
            startPosition.y = -canvasRect.rect.height;
            textContainer.anchoredPosition = startPosition;
            
            isScrolling = true;
        }
        
        // ��ʼ����
        StartCoroutine(ScrollText());
    }

    private IEnumerator ScrollText()
    {
        if (textContainer == null || canvasRect == null || !isScrolling) yield break;
        
        Vector3 startPosition = textContainer.anchoredPosition;
        Vector3 endPosition = textContainer.anchoredPosition;
        endPosition.y = canvasRect.rect.height + 200f; // ��������Ļ��������
        
        float elapsedTime = 0f;
        
        while (elapsedTime < scrollDuration && isScrolling)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / scrollDuration);
            
            // ʹ��ƽ����ֵʹ��������Ȼ
            textContainer.anchoredPosition = Vector3.Lerp(startPosition, endPosition, progress);
            yield return null;
        }
    }
    
    private void OnDestroy()
    {
        isScrolling = false;
        StopAllCoroutines();
    }
}