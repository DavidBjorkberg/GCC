using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public struct SpawnGoliathData : IComponentData
{
    public Entity goliathPrefab;
    public int nrOfGoliathsToSpawn;
    internal int nrOfSpawnedGoliaths;
}
