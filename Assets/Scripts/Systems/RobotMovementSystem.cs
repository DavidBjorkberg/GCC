using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine;
public class RobotMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        Entities.
            WithAll<FlyingRobotTag>().
            ForEach((ref Rotation rot, ref Translation trans, ref RobotMovementData robotMovementData) =>
        {
            if (robotMovementData.lerpValue < 1)
            {
                trans.Value = Vector3.Lerp(robotMovementData.startPos, robotMovementData.target, robotMovementData.lerpValue);
                rot.Value = Quaternion.LookRotation(robotMovementData.targetNormal, new float3(0, 1, 0));
                robotMovementData.lerpValue += deltaTime;
            }
        }).Run();
    }
}