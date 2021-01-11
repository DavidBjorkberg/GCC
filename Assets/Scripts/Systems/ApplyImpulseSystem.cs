using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

public class ApplyImpulseSystem : SystemBase
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
        Entities.WithStructuralChanges().
            WithAll<ApplyImpulseTag>().
            ForEach((Entity entity, ref PhysicsVelocity velocity, ref PhysicsGravityFactor gravityFactor, in Rotation rot, in PhysicsCollider physicsCollider, in DeadRobotData deadRobotData, in PhysicsMass mass) =>
            {
                Translation bulletTrans = EntityManager.GetComponentData<Translation>(deadRobotData.bullet);
                Translation trans = EntityManager.GetComponentData<Translation>(entity);
                EntityManager.SetComponentData(entity, PhysicsMass.CreateDynamic(new MassProperties()
                {
                    AngularExpansionFactor = mass.AngularExpansionFactor,
                    MassDistribution = new MassDistribution()
                    {
                        Transform = mass.Transform,
                        InertiaTensor = new float3(1,1,1)

                    },
                    Volume = 1
                }, 1));
                PhysicsMass newMass = EntityManager.GetComponentData<PhysicsMass>(entity);
                float3 impulse = math.normalize(bulletTrans.Value - trans.Value) * 10;
                gravityFactor.Value = 1;
                PhysicsComponentExtensions.ApplyExplosionForce(ref velocity, newMass, physicsCollider, trans, rot, 100, bulletTrans.Value, 5, 1.0f / 30.0f, new float3(0, 1, 0), 3, ForceMode.Impulse);
                EntityManager.RemoveComponent<ApplyImpulseTag>(entity);
            })
            .WithoutBurst()
            .Run();
    }
}