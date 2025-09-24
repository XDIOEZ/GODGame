using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameEndText : MonoBehaviour
{
    [Header("UI组件")]
    public Text deathCountText;
    public RectTransform textContainer;
    public RectTransform canvasRect;
    
    [Header("滚动设置")]
    [Tooltip("滚动时间(秒)")]
    public float scrollDuration = 20f;
    
[Header("文字内容")]
[TextArea(10, 20)]
public string endText = @"银河为证，星辰作陪

你已征服了这片星域
书写了属于自己的传奇

无数次的跌倒与爬起
见证了真正的勇者之心

从渺小的起点
到辉煌的终点
每一步都值得铭记

英雄，你的征途是星辰大海
而今，你已抵达彼岸

愿你的名字
在银河中永远闪耀

直到下一次冒险的开始
愿宇宙的风
永远在你身后轻拂...";
    private bool isScrolling = false;

    private void Start()
    {

        // 获取Canvas RectTransform（如果未指定则自动获取）
        if (canvasRect == null)
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                canvasRect = canvas.GetComponent<RectTransform>();
            }
        }

        // 获取玩家死亡次数并显示
        int deathCount = Checkpoint.GetPlayerDeathCount();

        // 获取玩家总游戏时间
        string playTime = PlayerPlayTimeManager.GetFormattedPlayTime();

        // 设置文字内容，包含死亡次数和游戏时间
        if (deathCountText != null)
        {
            deathCountText.text = $"游戏结束\n\n玩家死亡次数: {deathCount}\n游玩时间: {playTime}\n\n{endText}";
        }

        // 设置起始位置在屏幕底部以下
        if (textContainer != null && canvasRect != null)
        {
            Vector3 startPosition = textContainer.anchoredPosition;
            startPosition.y = -canvasRect.rect.height;
            textContainer.anchoredPosition = startPosition;
            
            isScrolling = true;
        }
        
        // 开始滚动
        StartCoroutine(ScrollText());
    }

    private IEnumerator ScrollText()
    {
        if (textContainer == null || canvasRect == null || !isScrolling) yield break;
        
        Vector3 startPosition = textContainer.anchoredPosition;
        Vector3 endPosition = textContainer.anchoredPosition;
        endPosition.y = canvasRect.rect.height + 200f; // 滚动到屏幕顶部以上
        
        float elapsedTime = 0f;
        
        while (elapsedTime < scrollDuration && isScrolling)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / scrollDuration);
            
            // 使用平滑插值使滚动更自然
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