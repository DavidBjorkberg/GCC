using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine;
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
        Entities.
            WithAll<DeadRobotTag>().
            ForEach((Entity entity, in ParentGoliathData parentGoliathData, in RobotMovementData movementData) =>
            {
                BuildMeshData buildMeshData = EntityManager.GetComponentData<BuildMeshData>(parentGoliathData.goliath);
                buildMeshData.freePolygons[movementData.claimedPolygon] = movementData.claimedPolygon;

                commandBuffer.SetComponent(parentGoliathData.goliath, buildMeshData);

                commandBuffer.DestroyEntity(entity);
            })
            .WithoutBurst()
            .Run();
    }

}