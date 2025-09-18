using UnityEngine;

public class FallingSoundController : MonoBehaviour
{
    [Header("�������")]
    public Rigidbody2D targetRigidbody;
    public Engine engine;

    [Header("׹����")]
    public AudioType fallingSoundType = AudioType.Full;
    public float minFallingSpeed = 3f;
    public float maxFallingSpeed = 15f;
    public float speedSmoothing = 5f;

    [Header("�ӳ�����")]
    public float startDelay = 0.1f;
    public float stopDelay = 0.5f;

    [Header("����ѡ��")]
    public bool debugMode = true;

    private float currentVerticalVelocity;
    private float smoothedVerticalVelocity;
    private bool isFalling;
    private bool wasFallingLastFrame; // ������һ֡��״̬
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
            Debug.LogWarning($"FallingSoundController on {gameObject.name}: δ�ҵ�Rigidbody2D�����");
        }
    }

    private void Update()
    {
        if (!hasValidRigidbody)
        {
            TryFindRigidbody();
            return;
        }

        // ���㴹ֱ�ٶȣ�ȡ����ֵ����Ϊ����ʱvelocity.yΪ��ֵ��
        currentVerticalVelocity = Mathf.Abs(targetRigidbody.velocity.y);
        smoothedVerticalVelocity = Mathf.Lerp(smoothedVerticalVelocity, currentVerticalVelocity, speedSmoothing * Time.deltaTime);

        // ������һ֡��״̬
        bool previousFallingState = isFalling;

        // ��鵱ǰ�Ƿ�׹��
        isFalling = CheckIfFalling();

        // ��������״̬
        HandleSoundState(previousFallingState);

        // ������һ֡״̬
        wasFallingLastFrame = isFalling;

        if (debugMode && Time.frameCount % 30 == 0)
        {
            Debug.Log($"�ٶ�: {currentVerticalVelocity:F1}, ƽ���ٶ�: {smoothedVerticalVelocity:F1}, �Ƿ�׹��: {isFalling}, ��Ч������: {soundShouldPlay}");
        }
    }

    private bool CheckIfFalling()
    {
        if (!hasValidRigidbody) return false;

        // ����ٶ��Ƿ�ﵽ��С׹���ٶ�
        if (smoothedVerticalVelocity < minFallingSpeed)
            return false;

        // ����Ƿ���kinematic
        if (targetRigidbody.isKinematic)
            return false;

        // ����Ƿ����������ƶ���velocity.yΪ��ֵ��
        if (targetRigidbody.velocity.y >= 0)
            return false;

        return true;
    }

    private void HandleSoundState(bool wasFalling)
    {
        // ���¼�ʱ��
        if (isFalling)
        {
            fallingTimer += Time.deltaTime;
            notFallingTimer = 0f;

            // ����Ƿ�Ӧ�ÿ�ʼ������Ч
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

            // ����Ƿ�Ӧ��ֹͣ������Ч
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
        if (debugMode) Debug.Log($"��ʼ����׹���������ٶ�: {smoothedVerticalVelocity:F1}m/s");
    }

    private void StopFallingSound()
    {
        EventController.RaiseOnStopAudio(fallingSoundType);
        if (debugMode) Debug.Log("ֹͣ����׹������");
    }

    private void TryFindRigidbody()
    {
        targetRigidbody = FindObjectOfType<Rigidbody2D>();
        hasValidRigidbody = targetRigidbody != null;

        if (hasValidRigidbody && debugMode)
        {
            Debug.Log($"�ҵ�Rigidbody2D: {targetRigidbody.gameObject.name}");
        }
    }

    // ǿ��ֹͣ��Ч
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
