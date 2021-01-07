using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public class AttachedRobotsData : IComponentData
{
    internal Entity[] attachedRobots;
}
