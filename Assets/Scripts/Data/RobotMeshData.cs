using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
[GenerateAuthoringComponent]
public class RobotMeshData : IComponentData
{
    internal RenderMesh renderMesh;
}
