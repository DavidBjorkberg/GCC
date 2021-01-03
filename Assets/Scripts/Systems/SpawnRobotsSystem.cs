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
        var entityCommandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer();

        Entities.
            WithAll<ConstructingGoliathTag>().
            ForEach((BuildMeshData buildMeshData, ref SpawnRobotData spawnRobotData, in Translation trans) =>
        {
            int totalNrOfPolygons = 6;
            if (spawnRobotData.nrOfAliveRobots < totalNrOfPolygons)
            {

                //Spawn
                var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

                for (int i = 0; i < totalNrOfPolygons; i++)
                {
                    Vector2 randomDir;
                    float randomDistance;
                    Vector2 randomPoint;
                    Vector3 spawnPos;
                    randomDir = UnityEngine.Random.insideUnitCircle.normalized;
                    randomDistance = UnityEngine.Random.Range(spawnRobotData.minSpawnDistance, spawnRobotData.maxSpawnDistance);
                    randomPoint = randomDir * randomDistance;

                    spawnPos = new Vector3(trans.Value.x + randomPoint.x, trans.Value.y, trans.Value.z + randomPoint.y);

                    var instance = entityCommandBuffer.Instantiate(spawnRobotData.robotPrefab);

                    entityCommandBuffer.SetComponent(instance, new Translation { Value = spawnPos });
                    spawnRobotData.nrOfAliveRobots++;

                    //Initialise
                    RobotMovementData robotMovementData;
                    robotMovementData.lerpValue = 0;
                    robotMovementData.startPos = spawnPos;
                    robotMovementData.claimedPolygon = GetAndClaimNextFreePolygon(buildMeshData.freePolygons);
                    robotMovementData.targetNormal = GetNormalOfPolygon(robotMovementData.claimedPolygon, buildMeshData.buildMesh);
                    robotMovementData.target = GetCenterOfPolygon(robotMovementData.claimedPolygon, buildMeshData.buildMesh);
                    robotMovementData.movementSpeed = 5;//SET MS HERE

                    entityCommandBuffer.SetComponent(instance, robotMovementData);
                    entityCommandBuffer.RemoveComponent(instance, typeof(UninitializedRobotTag));
                    entityCommandBuffer.AddComponent(instance, typeof(FlyingRobotTag));
                }
            }
        })
            .WithoutBurst()
            .Run();
        m_EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
    public Vector3 GetNormalOfPolygon(int index, Mesh buildMesh)
    {
        return buildMesh.normals[index * 4];
    }
    public int GetAndClaimNextFreePolygon(int[] freePolygons)
    {
        for (int i = freePolygons.Length - 1; i >= 0; i--)
        {
            if (freePolygons[i] != -1)
            {
                int freePolygonIndex = freePolygons[i];
                freePolygons[i] = -1;
                return freePolygonIndex;
            }
        }
        Debug.Log("Couldn't find free polygon");
        return -1;
    }
    public Vector3 GetCenterOfPolygon(int index, Mesh buildMesh)
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
}
