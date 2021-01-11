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
                Vector3 spawnPos = new Vector3(5 + spawnGoliathData.nrOfSpawnedGoliaths * 5, 0.0f, 0);

                GameObject navGO = GameObject.Instantiate(spawnGoliathData.goliathNavPrefab, spawnPos, quaternion.identity);

                GoliathNavData navData = new GoliathNavData();
                navData.navTransform = navGO.transform;
                navData.agent = navGO.GetComponent<NavMeshAgent>();
                navData.path = new NavMeshPath();
                Entity instance;
                if (spawnGoliathData.nrOfSpawnedGoliaths == 0)
                {
                    instance = commandBuffer.Instantiate(spawnGoliathData.goliathPrefab);
                    commandBuffer.SetComponent(instance, navData);
                    commandBuffer.SetComponent(instance, new Translation { Value = spawnPos });
                }
                else
                {
                    instance = commandBuffer.Instantiate(spawnGoliathData.goliathPrefab2);
                    commandBuffer.SetComponent(instance, navData);
                    commandBuffer.SetComponent(instance, new Translation { Value = spawnPos + new Vector3(0,0.5f,0) });
                }

                spawnGoliathData.nrOfSpawnedGoliaths++;
            }
        })
            .WithoutBurst()
            .Run();
    }
}
