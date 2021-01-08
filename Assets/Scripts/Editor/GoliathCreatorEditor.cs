using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GoliathCreator))]
public class GoliathCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GoliathCreator myTarget = (GoliathCreator)target;

        if (GUILayout.Button("Build Mesh"))
        {
            myTarget.CreateBody();
        }
        if (GUILayout.Button("Create Joint Links"))
        {
            myTarget.CreateJointLinks();
        }
    }
  
}

