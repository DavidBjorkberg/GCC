using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public class BuildMeshData : IComponentData
{
    public Mesh buildMesh;
    internal int[] freePolygons;
}

