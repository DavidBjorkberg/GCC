using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class InitializeGoliathSystem : SystemBase
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
            WithAll<UninitializedGoliathTag>()
            .ForEach((Entity entity, BuildMeshData buildMeshData) =>
        {
            int counter = 0;
            int nrOfPolygons = GetTotalNrOfPolygons(buildMeshData.buildMesh);
            buildMeshData.freePolygons = new int[nrOfPolygons];
            for (int i = nrOfPolygons - 1; i >= 0; i--)
            {
                buildMeshData.freePolygons[counter] = i;
                counter++;
            }
           // EntityManager.SetComponentData(entity, buildMeshData);
            entityCommandBuffer.RemoveComponent(entity, typeof(UninitializedGoliathTag));
            entityCommandBuffer.AddComponent(entity, typeof(ConstructingGoliathTag));
        })
            .WithoutBurst()
            .Run();
    }
    public int GetTotalNrOfPolygons(Mesh buildMesh)
    {
        return buildMesh.triangles.Length / 6;
    }
}