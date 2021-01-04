using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public struct ConstructData : IComponentData
{
    internal int nrOfRobotSlots;
    internal int currentNrOfAttachedRobots;
}
