using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine;
public class RobotMovementSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer();
        float deltaTime = Time.DeltaTime;
        float elapsedTime = (float)Time.ElapsedTime;
        Entities.
            WithAll<FlyingRobotTag>().
            ForEach((Entity entity, ref Rotation rot, ref Translation trans
            , ref RobotMovementData robotMovementData, in ParentGoliathData parentGoliathData) =>
        {
            if (robotMovementData.lerpValue < 1)
            {
                trans.Value = Vector3.Lerp(robotMovementData.startPos, robotMovementData.target, robotMovementData.lerpValue);
                rot.Value = Quaternion.LookRotation(robotMovementData.targetNormal, new float3(0, 1, 0));
                robotMovementData.lerpValue += deltaTime * 2;
            }
            else
            {
                ConstructData constructData = EntityManager.GetComponentData<ConstructData>(parentGoliathData.goliath);
                constructData.currentNrOfAttachedRobots++;
                EntityManager.SetComponentData(parentGoliathData.goliath, constructData);
                commandBuffer.RemoveComponent(entity, typeof(FlyingRobotTag));
                commandBuffer.AddComponent(entity, typeof(AttachedRobotTag));
            }
        })
            .WithoutBurst()
            .Run();
    }
}