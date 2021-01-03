using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public class Construct : SystemBase
{
    //public Mesh buildMesh;
    //private List<int> freePolygons = new List<int>();
    //protected override void OnCreate()
    //{
    //    for (int i = GetTotalNrOfPolygons() - 1; i >= 0; i--)
    //    {
    //        freePolygons.Add(i);
    //    }
    //}
    //public Vector3 GetCenterOfPolygon(int index)
    //{
    //    Vector3[] vertices = buildMesh.vertices;
    //    int[] triangles = buildMesh.triangles;
    //    Vector3 centerPos = Vector3.zero;

    //    index *= 6;
    //    centerPos += vertices[triangles[index]] * 100;
    //    centerPos += vertices[triangles[index + 1]] * 100;
    //    centerPos += vertices[triangles[index + 2]] * 100;
    //    centerPos += vertices[triangles[index + 3]] * 100;
    //    centerPos += vertices[triangles[index + 4]] * 100;
    //    centerPos += vertices[triangles[index + 5]] * 100;
    //    centerPos /= 6;
    //    return centerPos;
    //}
    //public Vector3 GetNormalOfPolygon(int index)
    //{
    //    return buildMesh.normals[index * 4];
    //}
    ///// <summary>
    ///// Removes the returned polygon from the free polygons list
    ///// </summary>
    ///// <returns></returns>
    //public int GetAndClaimNextFreePolygon()
    //{
    //    int freePolygonIndex = freePolygons[freePolygons.Count - 1];
    //    freePolygons.RemoveAt(freePolygons.Count - 1);

    //    return freePolygonIndex;
    //}
    //public int GetTotalNrOfPolygons()
    //{
    //    return buildMesh.triangles.Length / 6;
    //}
    //protected override void OnUpdate()
    //{

    //}
    //public void ReleasePolygon(int index)
    //{
    //    for (int i = freePolygons.Count - 1; i > 0; i--)
    //    {
    //        if (index > freePolygons[i])
    //        {
    //            freePolygons.Insert(i, index);
    //            break;
    //        }
    //    }
    //}
    protected override void OnUpdate()
    {
       
    }
}
