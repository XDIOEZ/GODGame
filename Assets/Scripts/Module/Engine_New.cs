using UnityEngine;

public class Engine_New : Engine
{
    public static Vector2 totalLinearVelocity;   // ����������ӵ����ٶ�
    public static float totalAngularVelocity;    // ����������ӵĽ��ٶ�

    private bool engineActive;

    private void Update()
    {
        //SimulationGravity();

        if (!engineActive || Fuel.fuel <= 0)
        {
            if (engineParticles != null) engineParticles.SetActive(false);
            return;
        }

        if (engineParticles != null) engineParticles.SetActive(true);

        Fuel.fuel -= (Time.deltaTime * Fuel.FuelCoefficient);

        // ����������ٶȹ���
        Vector2 linearContribution = transform.up * thrustForce;
        float angularContribution = (engineType == EngineType.Left ? -torqueForce : torqueForce);

        // �ۼӵ�ȫ��
        totalLinearVelocity += linearContribution;
        totalAngularVelocity += angularContribution;
    }

    private void FixedUpdate()
    {
       // StabilizeDirection();

        // ����̬�ۺϽ����ֵ�����壨ִֻ��һ�Σ����Է��� PlayerController��
        if (engineType == EngineType.Left) // ֻ��һ������ִ�У������ظ���ֵ��
        {
            rb.velocity = totalLinearVelocity;
            rb.angularVelocity = totalAngularVelocity;

            // ���㣬�ȴ���һ֡���¼���
            totalLinearVelocity = Vector2.zero;
            totalAngularVelocity = 0f;
        }
    }
}
