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
        float deltaTime = Time.DeltaTime;
        Entities.
            WithAll<DeadRobotTag>().
            ForEach((Entity entity, in ParentGoliathData parentGoliathData, in RobotDeathData robotDeathData) =>
            {
                SpawnRobotData spawnRobotData = EntityManager.GetComponentData<SpawnRobotData>(parentGoliathData.goliath);
                spawnRobotData.nrOfAliveRobots--;
                EntityManager.SetComponentData(parentGoliathData.goliath, spawnRobotData);

                BuildMeshData buildMeshData = EntityManager.GetComponentData<BuildMeshData>(parentGoliathData.goliath);
                buildMeshData.freePolygons[robotDeathData.claimedPolygon] = robotDeathData.claimedPolygon;
                EntityManager.SetComponentData(parentGoliathData.goliath, buildMeshData);

                commandBuffer.DestroyEntity(entity);
            })
            .WithoutBurst()
            .Run();
    }

}