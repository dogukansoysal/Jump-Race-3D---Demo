using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PathGenerator))]
public class PathGeneratorEditor : Editor
{
/*
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var pathGeneratorScript = (PathGenerator)target;
        if(GUILayout.Button("Generate Path"))
        {
            pathGeneratorScript.GeneratePath();
        }
    }
*/
}
