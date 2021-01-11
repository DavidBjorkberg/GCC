using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public class SpawnGoliathData : IComponentData
{
    public Entity goliathPrefab;
    public Entity goliathPrefab2;
    public GameObject goliathNavPrefab;
    public int nrOfGoliathsToSpawn;
    internal int nrOfSpawnedGoliaths;
}
