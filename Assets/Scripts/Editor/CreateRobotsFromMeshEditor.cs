using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CreateRobotsFromMesh))]
public class CreateRobotsFromMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CreateRobotsFromMesh myTarget = (CreateRobotsFromMesh)target;

        if (GUILayout.Button("Build mesh"))
        {
            for (int i = 0; i < myTarget.buildMesh.triangles.Length / 6; i++)
            {
                Vector3 pos = myTarget.transform.position + GetCenterOfPolygon(i, myTarget.buildMesh);
                GameObject robotGO = Instantiate(myTarget.robotPrefab, pos, Quaternion.identity, myTarget.gameObject.transform);

                robotGO.transform.rotation = Quaternion.LookRotation(GetNormalOfPolygon(i, myTarget.buildMesh), new Vector3(0, 1, 0));
            }
        }
    }
    public Vector3 GetCenterOfPolygon(int index, Mesh buildMesh)
    {
        Vector3[] vertices = buildMesh.vertices;
        int[] triangles = buildMesh.triangles;
        Vector3 centerPos = Vector3.zero;

        index *= 6;
        centerPos += vertices[triangles[index]];
        centerPos += vertices[triangles[index + 1]];
        centerPos += vertices[triangles[index + 2]];
        centerPos += vertices[triangles[index + 3]];
        centerPos += vertices[triangles[index + 4]];
        centerPos += vertices[triangles[index + 5]];
        centerPos /= 6;
        return centerPos;
    }
    public Vector3 GetNormalOfPolygon(int index, Mesh buildMesh)
    {
        Vector3[] normals = buildMesh.normals;
        int[] triangles = buildMesh.triangles;

        return normals[triangles[index * 6]];
    }
}

