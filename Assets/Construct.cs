using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construct : MonoBehaviour
{
    public Mesh buildMesh;
    private List<int> freePolygons = new List<int>();
    private void Awake()
    {
        int nrOfTriangles = buildMesh.triangles.Length;
        for (int i = (nrOfTriangles / 6) - 1; i > 0; i--)
        {
            freePolygons.Add(i);
        }
    }
    void Start()
    {
        //robots[0].MoveTo(transform.position + GetCenterOfPolygon(0), GetNormalOfPolygon(0));
    }

    Vector3 GetCenterOfPolygon(int index)
    {
        Vector3[] vertices = buildMesh.vertices;
        int[] triangles = buildMesh.triangles;
        Vector3 centerPos = Vector3.zero;

        index *= 6;
        centerPos += vertices[triangles[index]] * 100;
        centerPos += vertices[triangles[index + 1]] * 100;
        centerPos += vertices[triangles[index + 2]] * 100;
        centerPos += vertices[triangles[index + 3]] * 100;
        centerPos += vertices[triangles[index + 4]] * 100;
        centerPos += vertices[triangles[index + 5]] * 100;
        centerPos /= 6;
        return centerPos;
    }
    Vector3 GetNormalOfPolygon(int index)
    {
        return buildMesh.normals[index * 4];
    }

    int GetAndClaimNextFreePolygon()
    {
        if (freePolygons.Count <= 0)
        {
            print("Tried to get and claim free polygon. Shouldn't happen");
        }
        int freePolygonIndex = freePolygons[freePolygons.Count - 1];
        freePolygons.RemoveAt(freePolygonIndex);

        return freePolygonIndex;
    }
    public void ReleasePolygon(int index)
    {
        for (int i = freePolygons.Count - 1; i > 0; i--)
        {
            if (index > freePolygons[i])
            {
                freePolygons.Insert(i, index);
                break;
            }
        }
    }
}
