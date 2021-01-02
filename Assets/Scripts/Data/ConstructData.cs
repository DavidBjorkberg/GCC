using Unity.Entities;
using UnityEngine;
public struct ConstructData : IComponentData
{
    public bool isCompleted;
    public Mesh buildMesh;
    public int nrOfSpawnedRobots;
}
