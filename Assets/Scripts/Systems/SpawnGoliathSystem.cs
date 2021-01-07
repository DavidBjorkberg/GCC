using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
public class SpawnGoliathSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer();

        Entities.ForEach((SpawnGoliathData spawnGoliathData) =>
        {
            if (spawnGoliathData.nrOfSpawnedGoliaths < spawnGoliathData.nrOfGoliathsToSpawn)
            {
                Entity instance = commandBuffer.Instantiate(spawnGoliathData.goliathPrefab);
                var position = new Vector3(spawnGoliathData.nrOfSpawnedGoliaths * 5, 1.0f, 0);
                GameObject navGO = GameObject.Instantiate(spawnGoliathData.goliathNavPrefab, position - new Vector3(0, 1.0f, 0), quaternion.identity);

                GoliathNavData navData = new GoliathNavData();
                navData.navTransform = navGO.transform;
                navData.agent = navGO.GetComponent<NavMeshAgent>();
                navData.path = new NavMeshPath();
                commandBuffer.SetComponent(instance, new Translation { Value = position });
                commandBuffer.SetComponent(instance, navData);

                spawnGoliathData.nrOfSpawnedGoliaths++;
            }
        })
            .WithoutBurst()
            .Run();
    }
}
