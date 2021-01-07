using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.AI;

[GenerateAuthoringComponent]
public struct SpawnRobotData : IComponentData
{
    
    public Entity robotPrefab;
    public int minSpawnDistance;
    public int maxSpawnDistance;
    public float spawnCooldown;
    internal float spawnTimer;
    public int nrOfAliveRobots;
}
