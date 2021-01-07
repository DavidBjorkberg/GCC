using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public class SpawnRobotsSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer();
        float deltaTime = Time.DeltaTime;

        Entities.
            WithAll<ConstructingGoliathTag>().
            ForEach((BuildMeshData buildMeshData, Entity entity, ref SpawnRobotData spawnRobotData, in Translation trans) =>
        {
            spawnRobotData.spawnTimer -= deltaTime;

            bool hasFreePolygon = GetNrOfFreePolygons(buildMeshData) > 0;
            bool spawnCooldownReady = spawnRobotData.spawnTimer <= 0;
            if (hasFreePolygon && spawnCooldownReady)
            {
                ParentGoliathData goliathData;
                goliathData.goliath = entity;

                Entity instance = SpawnRobot(ref spawnRobotData, commandBuffer);
                Vector3 spawnPos = CalculateSpawnPos(spawnRobotData, trans.Value);

                commandBuffer.SetComponent(instance, goliathData);
                commandBuffer.SetComponent(instance, new Translation { Value = spawnPos });
            }
        })
            .WithoutBurst()
            .Run();

        m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
    Entity SpawnRobot(ref SpawnRobotData spawnRobotData, EntityCommandBuffer commandBuffer)
    {
        Entity instance = commandBuffer.Instantiate(spawnRobotData.robotPrefab);
        spawnRobotData.spawnTimer = spawnRobotData.spawnCooldown;
        return instance;
    }
    float3 CalculateSpawnPos(SpawnRobotData spawnRobotData, Vector3 goliathPos)
    {
        Vector2 randomDir;
        float randomDistance;
        Vector2 randomPoint;
        Vector3 spawnPos;
        randomDir = UnityEngine.Random.insideUnitCircle.normalized;
        randomDistance = UnityEngine.Random.Range(spawnRobotData.minSpawnDistance, spawnRobotData.maxSpawnDistance);
        randomPoint = randomDir * randomDistance;
        spawnPos = goliathPos + new Vector3(randomPoint.x, 0, randomPoint.y);
        return spawnPos;
    }

    int GetNrOfFreePolygons(BuildMeshData buildMeshData)
    {
        int nrOfFreePolygons = 0;

        for (int i = 0; i < buildMeshData.freePolygons.Length; i++)
        {
            if(buildMeshData.freePolygons[i] != -1)
            {
                nrOfFreePolygons++;
            }
        }
        return nrOfFreePolygons;
    }

}
