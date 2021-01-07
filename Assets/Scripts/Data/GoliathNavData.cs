using Unity.Entities;
using UnityEngine;
using UnityEngine.AI;

[GenerateAuthoringComponent]
public class GoliathNavData : IComponentData
{
    internal Transform navTransform;
    internal NavMeshAgent agent;
    internal NavMeshPath path;
}
