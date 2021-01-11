using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class InitializeGoliathSystem : SystemBase
{

    BeginInitializationEntityCommandBufferSystem commandBufferSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        
        var commandBuffer = commandBufferSystem.CreateCommandBuffer();
        Entities.
            WithAll<UninitializedGoliathTag>()
            .ForEach((Entity entity, BuildMeshData buildMeshData, AttachedRobotsData attachedRobotsData, ref ConstructData constructData) =>
        {
            int nrOfPolygons = GetTotalNrOfPolygons(buildMeshData.buildMesh);

            buildMeshData.freePolygons = new int[nrOfPolygons];
            for (int i = 0; i < nrOfPolygons; i++)
            {
                buildMeshData.freePolygons[i] = i;
            }
            attachedRobotsData.attachedRobots = new Entity[nrOfPolygons];
            constructData.nrOfRobotSlots = nrOfPolygons;
            commandBuffer.SetComponent(entity, buildMeshData);

            commandBuffer.RemoveComponent(entity, typeof(UninitializedGoliathTag));
            commandBuffer.AddComponent(entity, typeof(ConstructingGoliathTag));
        })
            .WithoutBurst()
            .Run();
    }
    public int GetTotalNrOfPolygons(Mesh buildMesh)
    {
        return buildMesh.triangles.Length / 6;
    }
}