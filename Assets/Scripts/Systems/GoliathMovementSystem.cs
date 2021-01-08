using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;
using Unity.Collections;
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

        float3 direction = new float3(1, 0, 1);
        Entities.
            WithAll<CompletedGoliathTag>().
            ForEach((Entity entity, GoliathNavData navData, ref Translation trans, ref Rotation rot) =>
            {
                if (!navData.agent.hasPath)
                {
                    Vector2 randomDir;
                    float randomDistance;
                    Vector2 randomPoint;
                    randomDir = UnityEngine.Random.insideUnitCircle.normalized;
                    randomDistance = UnityEngine.Random.Range(5, 10);
                    randomPoint = randomDir * randomDistance;
                    Vector3 point = new Vector3(randomPoint.x, 0, randomPoint.y);
                    navData.agent.CalculatePath(point, navData.path);
                    navData.agent.SetPath(navData.path);
                }

                trans.Value = navData.navTransform.position;
                rot.Value = navData.navTransform.rotation;
            })
            .WithoutBurst()
            .Run();
    }
}