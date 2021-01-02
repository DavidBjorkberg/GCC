using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using UnityEngine;

public class SpawnRobotsSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ConstructData constructData) => 
        {
            if(!constructData.isCompleted)
            {

            }
        
        }).Run();
    }
}
