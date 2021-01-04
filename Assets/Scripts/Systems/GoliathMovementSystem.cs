using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class GoliathMovementSystem : SystemBase
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
        float deltaTime = Time.DeltaTime;
        float3 direction = new float3(1, 0, 0);
        Entities.
            WithAll<CompletedGoliathTag>().
            ForEach((Entity entity, ref Translation trans) =>
            {
                trans.Value += direction * deltaTime * math.sin((float)Time.ElapsedTime);
            })
            .WithoutBurst()
            .Run();
    }
}