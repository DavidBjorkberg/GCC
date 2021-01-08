using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GoliathCreator : MonoBehaviour
{
    public GameObject robotPrefab;
    public Mesh buildMesh;


    public void CreateBody()
    {
        for (int i = 0; i < buildMesh.triangles.Length / 6; i++)
        {
            Vector3 pos = transform.position + GetCenterOfPolygon(i, buildMesh);
            GameObject robotGO = Instantiate(robotPrefab, pos, Quaternion.identity, gameObject.transform);
            robotGO.transform.rotation = Quaternion.LookRotation(GetNormalOfPolygon(i, buildMesh), new Vector3(0, 1, 0));
        }
    }
    struct Polygon
    {
        public int index;
        public List<Vector3> vertices;
        public bool HasMatchingVertex(Polygon other)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = 0; j < vertices.Count; j++)
                {
                    if (vertices[i] == other.vertices[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    public void CreateJointLinks()
    {
        Vector3[] vertices = buildMesh.vertices;
        int[] triangles = buildMesh.triangles;
        int totalNrOfConnectedPolygons = 0;
        int matchingNrOfPolygons = 0;
        List<List<Polygon>> connectedPolygons = new List<List<Polygon>>();
        for (int i = 0; i < triangles.Length / 6; i++)
        {
            List<Polygon> polygonList = new List<Polygon>();
            connectedPolygons.Add(polygonList);
        }
        List<Polygon> polygonsInMesh = new List<Polygon>();
        for (int i = 0; i < triangles.Length; i += 6)
        {
            Polygon newPolygon;
            newPolygon.vertices = new List<Vector3>();
            newPolygon.vertices.Add(vertices[triangles[i]]);
            newPolygon.vertices.Add(vertices[triangles[i + 1]]);
            newPolygon.vertices.Add(vertices[triangles[i + 2]]);
            newPolygon.vertices.Add(vertices[triangles[i + 5]]);
            newPolygon.index = Mathf.FloorToInt(i / 6);
            polygonsInMesh.Add(newPolygon);
        }



        for (int i = 0; i < polygonsInMesh.Count; i++)
        {
            for (int j = 0; j < polygonsInMesh.Count; j++)
            {
                if (polygonsInMesh[i].HasMatchingVertex(polygonsInMesh[j])
                    && !connectedPolygons[i].Contains(polygonsInMesh[j])
                    && polygonsInMesh[i].index != polygonsInMesh[j].index)
                {
                    connectedPolygons[i].Add(polygonsInMesh[j]);
                    totalNrOfConnectedPolygons++;
                }
            }
        }
        string path = Application.dataPath + "/GoliathJointData/" + name + ".txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
        for (int i = 0; i < connectedPolygons.Count; i++)
        {
            File.AppendAllText(path, "#\n");
            for (int j = 0; j < connectedPolygons[i].Count; j++)
            {
                File.AppendAllText(path, connectedPolygons[i][j].index + "\n");
            }
        }

        //for (int i = 0; i < connectedPolygons.Count; i++)
        //{
        //    List<int> connected = new List<int>();
        //    for (int j = 0; j < connectedPolygons[i].Count; j++)
        //    {
        //        connected.Add(connectedPolygons[i][j].index);
        //    }
        //    int a = connected[0];
        //    int b = connected[1];
        //    int c = connected[2];
        //    int d = connected[3];
        //    print("Polygon " + i + ": is connected to polygon " + a + " " + b + " " + c + " " + d);
        //}
        //print("Total: " + totalNrOfConnectedPolygons);
    }











    Vector3 GetCenterOfPolygon(int index, Mesh buildMesh)
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
    Vector3 GetNormalOfPolygon(int index, Mesh buildMesh)
    {
        Vector3[] normals = buildMesh.normals;
        int[] triangles = buildMesh.triangles;

        return normals[triangles[index * 6]];
    }
}

