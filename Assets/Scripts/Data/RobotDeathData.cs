using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct RobotDeathData : IComponentData
{
    internal int claimedPolygon;
}
