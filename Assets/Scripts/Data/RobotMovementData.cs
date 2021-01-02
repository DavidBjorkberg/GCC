using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct RobotMovementData : IComponentData
{
    public float movementSpeed;
    internal float3 target;
}
