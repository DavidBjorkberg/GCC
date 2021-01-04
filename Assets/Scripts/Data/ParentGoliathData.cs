using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct ParentGoliathData : IComponentData
{
    public Entity goliath;
}
