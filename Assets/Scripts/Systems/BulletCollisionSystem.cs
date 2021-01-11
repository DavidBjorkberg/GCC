using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class BulletCollisionSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem commandBuffer;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        commandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    struct BulletCollisionSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<RobotTag> allRobots;
        [ReadOnly] public ComponentDataFromEntity<BulletTag> allBullets;
        public EntityCommandBuffer entityCommandBuffer;
        public void Execute(TriggerEvent triggerEvent)
        {
            bool entityAAllRobots = allRobots.HasComponent(triggerEvent.EntityA);
            bool entityBAllRobots = allRobots.HasComponent(triggerEvent.EntityB);
            bool entityAAllBullets = allBullets.HasComponent(triggerEvent.EntityA);
            bool entityBAllBullets = allBullets.HasComponent(triggerEvent.EntityB);
            if (entityAAllRobots && entityBAllRobots)
            {
                return;
            }
            if (entityAAllBullets && entityBAllBullets)
            {
                return;
            }
            if (entityAAllBullets && entityBAllRobots)
            {
                DeadRobotData deadRobotData;
                deadRobotData.bullet = triggerEvent.EntityA;
                entityCommandBuffer.SetComponent(triggerEvent.EntityB, deadRobotData);
                entityCommandBuffer.RemoveComponent(triggerEvent.EntityB, typeof(RobotTag));
                entityCommandBuffer.AddComponent(triggerEvent.EntityB, typeof(DeadRobotTag));
            }
            else if (entityAAllRobots && entityBAllBullets)
            {
                DeadRobotData deadRobotData;
                deadRobotData.bullet = triggerEvent.EntityB;
                entityCommandBuffer.SetComponent(triggerEvent.EntityA, deadRobotData);
                entityCommandBuffer.RemoveComponent(triggerEvent.EntityA, typeof(RobotTag));
                entityCommandBuffer.AddComponent(triggerEvent.EntityA, typeof(DeadRobotTag));
            }
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new BulletCollisionSystemJob();
        job.allRobots = GetComponentDataFromEntity<RobotTag>(true);
        job.allBullets = GetComponentDataFromEntity<BulletTag>(true);
        job.entityCommandBuffer = commandBuffer.CreateCommandBuffer();
        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        commandBuffer.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }
}
