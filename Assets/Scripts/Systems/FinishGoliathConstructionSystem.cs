using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class FinishGoliathConstructionSystem : SystemBase
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
            WithAll<ConstructingGoliathTag>().
            ForEach((Entity entity, in ConstructData constructData) =>
        {
            if(constructData.currentNrOfAttachedRobots >= constructData.nrOfRobotSlots)
            {
                commandBuffer.RemoveComponent(entity, typeof(ConstructingGoliathTag));
                commandBuffer.AddComponent(entity, typeof(CompletedGoliathTag));
            }
            if (constructData.currentNrOfAttachedRobots > constructData.nrOfRobotSlots)
            {
                Debug.Log("WRONG");
            }

        })
            .WithoutBurst()
            .Run();
    }
}
