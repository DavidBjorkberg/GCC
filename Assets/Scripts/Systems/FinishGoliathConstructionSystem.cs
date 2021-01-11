using Unity.Entities;
using Unity.Jobs;
using System.IO;
using Unity.Physics.Authoring;
using UnityEngine;

public class FinishGoliathConstructionSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem commandBufferSystem;
    GameObjectConversionSystem conversionSystem;
    protected override void OnCreate()
    {
        base.OnCreate();
        conversionSystem = World.GetOrCreateSystem<EndJointConversionSystem>();
        commandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        var commandBuffer = commandBufferSystem.CreateCommandBuffer();

        Entities.
            WithAll<ConstructingGoliathTag>().
            ForEach((Entity entity, AttachedRobotsData attachedRobotsData, NeighbourLogData logData, in ConstructData constructData) =>
        {
            if (constructData.currentNrOfAttachedRobots >= constructData.nrOfRobotSlots)
            {

                //conversionSystem.World.GetOrCreateSystem<EndJointConversionSystem>().CreateJointEntity(
                //    this,
                //    new Unity.Physics.PhysicsConstrainedBodyPair(entity, attachedRobotsData.attachedRobots[0], false),
                //    Unity.Physics.PhysicsJoint.CreateFixed(Unity.Mathematics.RigidTransform.identity, Unity.Mathematics.RigidTransform.identity));
                //string path = Application.dataPath + "/GoliathNeighbourLogs/" + logData.logName + ".txt";
                //if (File.Exists(path))
                //{
                //    string[] lines = File.ReadAllLines(path);
                //    for (int i = 0; i < lines.Length; i += 4)
                //    {
                //        RobotNeighboursData neighbourData = EntityManager.GetComponentObject<RobotNeighboursData>(attachedRobotsData.attachedRobots[i / 4]);
                //        for (int j = 0; j < 4; j++)
                //        {
                //            neighbourData.neighbours[j] = attachedRobotsData.attachedRobots[int.Parse(lines[i + j])];
                //        }
                //        commandBuffer.SetComponent(attachedRobotsData.attachedRobots[i / 4 ], neighbourData);
                //    }
                //}
                //else
                //{
                //    Debug.Log("Can't find neighbour log");
                //}


                commandBuffer.RemoveComponent(entity, typeof(ConstructingGoliathTag));
                commandBuffer.AddComponent(entity, typeof(CompletedGoliathTag));
            }
            if (constructData.currentNrOfAttachedRobots > constructData.nrOfRobotSlots)
            {
                Debug.Log("Error");
            }
        })
            .WithoutBurst()
            .Run();
    }
}
