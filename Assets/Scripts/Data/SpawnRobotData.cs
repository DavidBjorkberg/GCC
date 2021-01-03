using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public struct SpawnRobotData : IComponentData
{
    public int minSpawnDistance;
    public int maxSpawnDistance;
    internal int nrOfAliveRobots;
    public Entity robotPrefab;
}
