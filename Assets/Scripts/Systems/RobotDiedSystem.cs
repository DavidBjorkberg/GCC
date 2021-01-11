using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine;
using Unity.Physics;
using Unity.Physics.Extensions;
public class RobotDiedSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        var commandBuffer = commandBufferSystem.CreateCommandBuffer();
        Entities.WithStructuralChanges().
            WithAll<DeadRobotTag>().
            ForEach((Entity entity,  in DeadRobotData deadRobotData, in RobotMovementData movementData) =>
            {
                EntityManager.AddComponent<PhysicsVelocity>(entity);

                commandBuffer.RemoveComponent<DeadRobotTag>(entity);
                commandBuffer.AddComponent<ApplyImpulseTag>(entity);
                //commandBuffer.DestroyEntity(entity);
            })
            .WithoutBurst()
            .Run();
    }

}