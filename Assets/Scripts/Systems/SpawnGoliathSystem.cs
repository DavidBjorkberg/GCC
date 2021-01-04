using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public class SpawnGoliathSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

        Entities.ForEach((int entityInQueryIndex, ref SpawnGoliathData spawnGoliathData) =>
        {
            if (spawnGoliathData.nrOfSpawnedGoliaths < spawnGoliathData.nrOfGoliathsToSpawn)
            {
                Entity instance = commandBuffer.Instantiate(entityInQueryIndex, spawnGoliathData.goliathPrefab);

                var position = new Vector3(spawnGoliathData.nrOfSpawnedGoliaths * 5, 0, 0);
                commandBuffer.SetComponent(entityInQueryIndex, instance, new Translation { Value = position });
                spawnGoliathData.nrOfSpawnedGoliaths++;
            }
        }).ScheduleParallel();
        m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}
