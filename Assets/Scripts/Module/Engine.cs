using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Engine : MonoBehaviour
{
    public enum EngineType { Left, Right }

    [Header("����")]
    public PlayerController controller; // Inspector ���� PlayerController
    public Button engineButton;         // ÿ�������Ӧһ����ť
    public Rigidbody2D rb;
    public GameObject engineParticles;

    [Header("�ƽ�����")]
    public float thrustForce = 5f;      // ������������
    public float torqueForce = 5f;

    [Header("��������")]
    public EngineType engineType;

    [Header("������ϵͳ Action")]
    public string inputActionName = "LeftFly"; // ���� Inspector �޸�

    private bool engineActive;

    private void Awake()
    {
        if (rb == null && controller != null)
            rb = controller.rb;

        // ���ݱ��������Զ��ж���������
        float localX = transform.localPosition.x;
        engineType = localX >= 0 ? EngineType.Right : EngineType.Left;

        // UI��ť�¼�
        AddButtonEvents(engineButton, () => engineActive = true, () => engineActive = false);

        // ������ϵͳ����
        if (controller != null && !string.IsNullOrEmpty(inputActionName))
        {
            var action = controller.playerInput.actions[inputActionName];
            if (action != null)
            {
                action.started += ctx => engineActive = true;
                action.canceled += ctx => engineActive = false;
            }
            else
            {
                Debug.LogWarning($"InputAction '{inputActionName}' not found on PlayerInput.");
            }
        }
    }

    private void Update()
    {
        // ���ӿ���
        if (engineParticles != null)
            engineParticles.SetActive(engineActive);

        if (!engineActive) return;

        // ֡���޹ص�������Ť��
        float delta = Time.deltaTime; // ��ǰ֡ʱ����

        // ����
        rb.AddForce(transform.up * thrustForce * delta, ForceMode2D.Force);

        // Ť��
        float torque = (engineType == EngineType.Left ? -torqueForce : torqueForce) * delta;
        rb.AddTorque(torque, ForceMode2D.Force);
    }



    private void AddButtonEvents(Button button, System.Action onDown, System.Action onUp)
    {
        if (button == null) return;

        var trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear();

        var entryDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entryDown.callback.AddListener((_) => onDown());
        trigger.triggers.Add(entryDown);

        var entryUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entryUp.callback.AddListener((_) => onUp());
        trigger.triggers.Add(entryUp);

        var entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((_) => onUp());
        trigger.triggers.Add(entryExit);
    }
}
