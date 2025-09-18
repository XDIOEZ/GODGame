using UnityEngine;

public class FallingSoundController : MonoBehaviour
{
    [Header("组件引用")]
    public Rigidbody2D targetRigidbody;
    public Engine engine;

    [Header("坠落检测")]
    public AudioType fallingSoundType = AudioType.Full;
    public float minFallingSpeed = 3f;
    public float maxFallingSpeed = 15f;
    public float speedSmoothing = 5f;

    [Header("延迟设置")]
    public float startDelay = 0.1f;
    public float stopDelay = 0.5f;

    [Header("调试选项")]
    public bool debugMode = true;

    private float currentVerticalVelocity;
    private float smoothedVerticalVelocity;
    private bool isFalling;
    private bool wasFallingLastFrame; // 跟踪上一帧的状态
    private bool soundShouldPlay;
    private float fallingTimer;
    private float notFallingTimer;
    private bool hasValidRigidbody;

    private void Awake()
    {
        if (targetRigidbody == null)
        {
            targetRigidbody = GetComponent<Rigidbody2D>();
            if (targetRigidbody == null)
            {
                targetRigidbody = GetComponentInParent<Rigidbody2D>();
            }
            if (targetRigidbody == null)
            {
                targetRigidbody = FindObjectOfType<Rigidbody2D>();
            }
        }

        hasValidRigidbody = targetRigidbody != null;

        if (!hasValidRigidbody && debugMode)
        {
            Debug.LogWarning($"FallingSoundController on {gameObject.name}: 未找到Rigidbody2D组件。");
        }
    }

    private void Update()
    {
        if (!hasValidRigidbody)
        {
            TryFindRigidbody();
            return;
        }

        // 计算垂直速度（取绝对值，因为下落时velocity.y为负值）
        currentVerticalVelocity = Mathf.Abs(targetRigidbody.velocity.y);
        smoothedVerticalVelocity = Mathf.Lerp(smoothedVerticalVelocity, currentVerticalVelocity, speedSmoothing * Time.deltaTime);

        // 保存上一帧的状态
        bool previousFallingState = isFalling;

        // 检查当前是否坠落
        isFalling = CheckIfFalling();

        // 处理声音状态
        HandleSoundState(previousFallingState);

        // 更新上一帧状态
        wasFallingLastFrame = isFalling;

        if (debugMode && Time.frameCount % 30 == 0)
        {
            Debug.Log($"速度: {currentVerticalVelocity:F1}, 平滑速度: {smoothedVerticalVelocity:F1}, 是否坠落: {isFalling}, 音效播放中: {soundShouldPlay}");
        }
    }

    private bool CheckIfFalling()
    {
        if (!hasValidRigidbody) return false;

        // 检查速度是否达到最小坠落速度
        if (smoothedVerticalVelocity < minFallingSpeed)
            return false;

        // 检查是否不是kinematic
        if (targetRigidbody.isKinematic)
            return false;

        // 检查是否正在向下移动（velocity.y为负值）
        if (targetRigidbody.velocity.y >= 0)
            return false;

        return true;
    }

    private void HandleSoundState(bool wasFalling)
    {
        // 更新计时器
        if (isFalling)
        {
            fallingTimer += Time.deltaTime;
            notFallingTimer = 0f;

            // 检查是否应该开始播放音效
            if (!soundShouldPlay && fallingTimer >= startDelay)
            {
                StartFallingSound();
                soundShouldPlay = true;
            }
        }
        else
        {
            notFallingTimer += Time.deltaTime;
            fallingTimer = 0f;

            // 检查是否应该停止播放音效
            if (soundShouldPlay && notFallingTimer >= stopDelay)
            {
                StopFallingSound();
                soundShouldPlay = false;
            }
        }
    }

    private void StartFallingSound()
    {
        EventController.RaiseOnPlayAudio(fallingSoundType);
        if (debugMode) Debug.Log($"开始播放坠落声音，速度: {smoothedVerticalVelocity:F1}m/s");
    }

    private void StopFallingSound()
    {
        EventController.RaiseOnStopAudio(fallingSoundType);
        if (debugMode) Debug.Log("停止播放坠落声音");
    }

    private void TryFindRigidbody()
    {
        targetRigidbody = FindObjectOfType<Rigidbody2D>();
        hasValidRigidbody = targetRigidbody != null;

        if (hasValidRigidbody && debugMode)
        {
            Debug.Log($"找到Rigidbody2D: {targetRigidbody.gameObject.name}");
        }
    }

    // 强制停止音效
    public void ForceStopFallingSound()
    {
        if (soundShouldPlay)
        {
            StopFallingSound();
            soundShouldPlay = false;
        }
    }

    public bool IsPlayingFallingSound()
    {
        return soundShouldPlay;
    }

    public float GetCurrentFallingSpeed()
    {
        return smoothedVerticalVelocity;
    }
}
