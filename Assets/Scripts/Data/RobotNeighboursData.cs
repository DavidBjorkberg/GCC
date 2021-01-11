using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public class  RobotNeighboursData : IComponentData
{
    internal Entity[] neighbours = new Entity[4];
}


