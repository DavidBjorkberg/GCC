using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class InitializeRobotSystem : SystemBase
{
    public EntityCommandBuffer entityCommandBuffer;
    public EntityCommandBufferSystem commandBufferSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        Entities.
            WithAll<UninitializedRobotTag>().
            ForEach((Entity entity, BuildMeshData buildMeshData,ref RobotMovementData robotMovementData, in Translation trans) =>
        {
            robotMovementData.lerpValue = 0;
            robotMovementData.startPos = trans.Value;
            robotMovementData.claimedPolygon = GetAndClaimNextFreePolygon(buildMeshData.freePolygons);
            robotMovementData.targetNormal = GetNormalOfPolygon(robotMovementData.claimedPolygon, buildMeshData.buildMesh);
            robotMovementData.target = GetCenterOfPolygon(robotMovementData.claimedPolygon, buildMeshData.buildMesh);
            EntityManager.SetComponentData(entity, robotMovementData);
            entityCommandBuffer.RemoveComponent(entity, typeof(UninitializedRobotTag));
            entityCommandBuffer.AddComponent(entity, typeof(FlyingRobotTag));

        })
            .WithoutBurst()
            .Run();
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