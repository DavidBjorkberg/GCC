using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;

public class InitializeRobotSystem : SystemBase
{
    public BeginInitializationEntityCommandBufferSystem commandBufferSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        var commandBuffer = commandBufferSystem.CreateCommandBuffer();

        Entities.
            WithAll<UninitializedRobotTag>().
            ForEach((Entity entity,  ref RobotMovementData robotMovementData, in Translation trans, in ParentGoliathData goliathData) =>
        {
            BuildMeshData buildMeshData = EntityManager.GetComponentObject<BuildMeshData>(goliathData.goliath);
            float3 goliathPos = EntityManager.GetComponentData<Translation>(goliathData.goliath).Value;

            robotMovementData.lerpValue = 0;
            robotMovementData.startPos = trans.Value;
            robotMovementData.claimedPolygon = GetAndClaimNextFreePolygon(buildMeshData.freePolygons);
            robotMovementData.targetNormal = GetNormalOfPolygon(robotMovementData.claimedPolygon, buildMeshData.buildMesh);
            robotMovementData.target = goliathPos + GetCenterOfPolygon(robotMovementData.claimedPolygon, buildMeshData.buildMesh);
            robotMovementData.movementSpeed = 5;

            commandBuffer.AddComponent(entity, new Parent { Value = goliathData.goliath });
            commandBuffer.AddComponent(entity, new LocalToParent { });

            commandBuffer.RemoveComponent(entity, typeof(UninitializedRobotTag));
            commandBuffer.AddComponent(entity, typeof(FlyingRobotTag));
        })
            .WithoutBurst()
            .Run();
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
        Vector3 centerPos = new Vector3(0, 0, 0);

        index *= 6;
        centerPos += vertices[triangles[index]];
        centerPos += vertices[triangles[index + 1]];
        centerPos += vertices[triangles[index + 2]];
        centerPos += vertices[triangles[index + 3]];
        centerPos += vertices[triangles[index + 4]];
        centerPos += vertices[triangles[index + 5]];
        centerPos /= 6.0f;
        return centerPos;
    }
}