using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Engine : MonoBehaviour
{
    public enum EngineType { Left, Right }

    [Header("����ϵ������")]
    public ParameterFallingspeed fallingspeed;

    [Header("��������")]
    public ParameterEngine Thrust;

    [Header("��������key��")]
    public string ThrustKey;
    [Header("��ǰȡkey��������")]
    public ThrustData CurrentTrustData ;

    [Header("����")]
    public PlayerController controller; // Inspector ���� PlayerController
    public Button engineButton;         // ÿ�������Ӧһ����ť
    public Rigidbody2D rb;
    public GameObject engineParticles;

    [Header("��ǰʣ��ȼ��")]
    public ParameterFuel Fuel;

    [Header("��ǰ��������")]
    private float FallingSpeed=1;

    [Header("�ƽ�����")]
    public float thrustForce = 5f;      // ������������
    public float torqueForce = 5f;

    //��������ֵ
    private float gravityForce = 200;

    public float stabilizationFactor = 0f; // ����ϵ����Խ�����Խ�죬�����1-5���ԣ�
    public float maxStabilizationTorque; // ������Ť�أ��������������

    [Header("��������")]
    public EngineType engineType;

    [Header("������ϵͳ Action")]
    public string inputActionName = "LeftFly"; // ���� Inspector �޸�

    private bool engineActive;

    private void Awake()
    {
        FallingSpeed = fallingspeed.Fallingspeed;
        stabilizationFactor = fallingspeed.ReturnCoefficient;
        maxStabilizationTorque = fallingspeed.ReturnCoefficient;

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
        SimulationGravity();
        // ���ӿ���
        if (engineParticles != null)
        {
            engineParticles.SetActive(engineActive);
            if (Fuel.fuel <= 0)
            {
                engineParticles.SetActive(false);
            }
        }

        if (!engineActive) return;

        //ȼ�Ϻľ�
        if (Fuel.fuel<=0)
            return;

        //�����ã����ڱ�ɾ��
        //if (Fuel.fuel < 100)
        //{
        //    EventManager.Instance.Emit(new ParameterFallingspeed(FallSpeed: 3));
        //    FallingSpeed = fallingspeed.Fallingspeed;
        //}
        //�����ã����ڱ�ɾ��

        // ֡���޹ص�������Ť��
        float delta = Time.deltaTime; // ��ǰ֡ʱ����
        Fuel.fuel -= (Time.deltaTime *Fuel.FuelCoefficient);

        // ����
        rb.AddForce(transform.up * (thrustForce) * delta, ForceMode2D.Force);

        // Ť��
        float torque = (engineType == EngineType.Left ? -torqueForce : torqueForce) * delta;
        rb.AddTorque(torque, ForceMode2D.Force);
    }
    private void FixedUpdate()
    {
        StabilizeDirection();
    }

    private void OnValidate()
    {
        // У�������Ƿ����
        if (Thrust == null)
        {
            CurrentTrustData = default; // ��յ�ǰ����
            return;
        }

        // ��keyΪ��ʱ��յ�ǰ����
        if (string.IsNullOrEmpty(ThrustKey))
        {
            CurrentTrustData = default;
            return;
        }

        // ���ֵ��в�ѯkey��Ӧ��ThrustData����ֵ
        if (Thrust.TryGetThrustData(ThrustKey, out ThrustData data))
        {
            CurrentTrustData = data;
            thrustForce = CurrentTrustData.ThrustPower;
            torqueForce = CurrentTrustData.TorqueForcePower;
            engineType = CurrentTrustData.EngineType;
            FallingSpeed = fallingspeed.Fallingspeed;
        }
        else
        {
            // ��key�����ڣ�������ݲ���ʾ
            CurrentTrustData = default;
            Debug.LogWarning($"���������в�����key��{ThrustKey}");
        }
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


    //ģ���������÷���(��׹�ٶȡ�������ϵ��ר��)
    private void SimulationGravity()
    {
        FallingSpeed = fallingspeed.Fallingspeed;
        float delta = Time.deltaTime; // ��ǰ֡ʱ����
        rb.AddForce(Vector2.down * (gravityForce) * delta*FallingSpeed, ForceMode2D.Force);
    }

    // �����ȶ��߼����û���������������Ϸ���
    private void StabilizeDirection()
    {
        // 1. ��ȡ��ǰ����ĳ��򣨾ֲ��Ϸ���
        Vector2 currentUp = transform.up;

        // 2. ���㵱ǰ�������������Ϸ���Vector2.up���ļнǣ��Ƕ��ƣ�-180~180��
        float angleToUp = Vector2.SignedAngle(-currentUp, Vector2.up);

        // 3. ���ݼнǼ�������Ť�أ��ǶȲ�Խ��������Խǿ�����Ի���ϵ����
        stabilizationFactor = fallingspeed.ReturnCoefficient;
        float correctionTorque = angleToUp * stabilizationFactor*0.01f;

        // 4. �������Ť�أ�����������ͣ���ѡ��������ӣ�
        correctionTorque = Mathf.Clamp(correctionTorque, -maxStabilizationTorque, maxStabilizationTorque);

        // 5. ʩ������Ť�أ�ע�⣺Ť�ط�����ǶȲ��෴�����ܻ�����
        rb.AddTorque(-correctionTorque * Time.fixedDeltaTime, ForceMode2D.Force);
    }
}
