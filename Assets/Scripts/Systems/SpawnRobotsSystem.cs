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
            ForEach((BuildMeshData buildMeshData, ref Entity entity, ref SpawnRobotData spawnRobotData, in Translation trans) =>
        {
            int totalNrOfPolygons = GetTotalNrOfPolygons(buildMeshData.buildMesh);
            spawnRobotData.spawnTimer -= deltaTime;

            bool hasSpawnedEnoughRobots = spawnRobotData.nrOfAliveRobots >= totalNrOfPolygons;
            bool spawnCooldownReady = spawnRobotData.spawnTimer <= 0;
            if (!hasSpawnedEnoughRobots && spawnCooldownReady)
            {
                Vector3 spawnPos = CalculateSpawnPos(spawnRobotData, trans.Value);
                Entity instance = SpawnRobot(ref spawnRobotData, commandBuffer);
                InitialiseRobot(instance, entity, spawnPos, trans.Value, buildMeshData, commandBuffer);

                commandBuffer.RemoveComponent(instance, typeof(UninitializedRobotTag));
                commandBuffer.AddComponent(instance, typeof(FlyingRobotTag));
            }
        })
            .WithoutBurst()
            .Run();

        m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
    Entity SpawnRobot(ref SpawnRobotData spawnRobotData, EntityCommandBuffer commandBuffer)
    {
        Entity instance = commandBuffer.Instantiate(spawnRobotData.robotPrefab);
        spawnRobotData.nrOfAliveRobots++;
        spawnRobotData.spawnTimer = spawnRobotData.spawnCooldown;
        return instance;
    }
    void InitialiseRobot(Entity instance, Entity goliath, float3 spawnPos, float3 goliathPos, BuildMeshData buildMeshData, EntityCommandBuffer commandBuffer)
    {
        RobotMovementData robotMovementData;
        robotMovementData.lerpValue = 0;
        robotMovementData.startPos = spawnPos;
        robotMovementData.claimedPolygon = GetAndClaimNextFreePolygon(buildMeshData.freePolygons);
        robotMovementData.targetNormal = GetNormalOfPolygon(robotMovementData.claimedPolygon, buildMeshData.buildMesh);
        robotMovementData.target = goliathPos + GetCenterOfPolygon(robotMovementData.claimedPolygon, buildMeshData.buildMesh);
        robotMovementData.movementSpeed = 5;

        ParentGoliathData parentGoliathData;
        parentGoliathData.goliath = goliath;

        commandBuffer.SetComponent(instance, new Translation { Value = spawnPos });
        commandBuffer.AddComponent(instance, new Parent { Value = goliath });
        commandBuffer.AddComponent(instance, new LocalToParent { });
        commandBuffer.SetComponent(instance, robotMovementData);
        commandBuffer.SetComponent(instance, parentGoliathData);
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
    public float3 GetNormalOfPolygon(int index, Mesh buildMesh)
    {
        Vector3[] normals = buildMesh.normals;
        int[] triangles = buildMesh.triangles;

        return normals[triangles[index * 6]];
    }
    public int GetAndClaimNextFreePolygon(int[] freePolygons)
    {
        for (int i = 0; i < freePolygons.Length; i++)
        {
            if (freePolygons[i] != -1)
            {
                freePolygons[i] = -1;
                return i;
            }
        }
        Debug.Log("Couldn't find free polygon");
        return -1;
    }
    public float3 GetCenterOfPolygon(int index, Mesh buildMesh)
    {
        Vector3[] vertices = buildMesh.vertices;
        int[] triangles = buildMesh.triangles;
        Vector3 centerPos = Vector3.zero;

        index *= 6;
        centerPos += vertices[triangles[index]] * 100;
        centerPos += vertices[triangles[index + 1]] * 100;
        centerPos += vertices[triangles[index + 2]] * 100;
        centerPos += vertices[triangles[index + 3]] * 100;
        centerPos += vertices[triangles[index + 4]] * 100;
        centerPos += vertices[triangles[index + 5]] * 100;
        centerPos /= 6;
        return centerPos;
    }
    public int GetTotalNrOfPolygons(Mesh buildMesh)
    {
        return buildMesh.triangles.Length / 6;
    }

}
