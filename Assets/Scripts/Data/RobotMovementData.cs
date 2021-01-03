using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct RobotMovementData : IComponentData
{
    public float movementSpeed;
    internal float lerpValue;
    internal float3 target;
    internal float3 startPos;
    internal float3 targetNormal;
    internal int claimedPolygon;
}
