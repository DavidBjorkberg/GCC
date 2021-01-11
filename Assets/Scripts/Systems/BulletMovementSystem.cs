using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
public class BulletMovementSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer();

        Entities.ForEach((BulletData bulletData, ref Translation trans) =>
        {
            trans.Value += new float3(bulletData.direction) * 0.3f;
        })
            .WithoutBurst()
            .Run();
    }
}
