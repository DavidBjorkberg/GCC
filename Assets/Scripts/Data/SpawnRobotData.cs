using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public struct SpawnRobotData : IComponentData
{
    internal int nrOfAliveRobots;
    public Entity robotPrefab;
}
