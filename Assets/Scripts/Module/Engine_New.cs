using UnityEngine;

public class Engine_New : Engine
{
    public static Vector2 totalLinearVelocity;   // 所有引擎叠加的线速度
    public static float totalAngularVelocity;    // 所有引擎叠加的角速度

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

        // 单个引擎的速度贡献
        Vector2 linearContribution = transform.up * thrustForce;
        float angularContribution = (engineType == EngineType.Left ? -torqueForce : torqueForce);

        // 累加到全局
        totalLinearVelocity += linearContribution;
        totalAngularVelocity += angularContribution;
    }

    private void FixedUpdate()
    {
       // StabilizeDirection();

        // 将静态聚合结果赋值给刚体（只执行一次，可以放在 PlayerController）
        if (engineType == EngineType.Left) // 只让一个引擎执行（避免重复赋值）
        {
            rb.velocity = totalLinearVelocity;
            rb.angularVelocity = totalAngularVelocity;

            // 清零，等待下一帧重新计算
            totalLinearVelocity = Vector2.zero;
            totalAngularVelocity = 0f;
        }
    }
}
