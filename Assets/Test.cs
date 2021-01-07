using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    private void Update()
    {
        Mesh mesh = new Mesh();
        for (int i = 0; i < meshFilter.mesh.vertices.Length; i++)
        {
           // Debug.Log(meshRenderer.sharedMesh.vertices[i]);
          
            Debug.Log(meshFilter.mesh.vertices[i]);
        }
    }
}
