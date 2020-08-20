using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class PathGenerator : MonoBehaviour
{
    public GameObject JumpingPadPrefab;

    public void GeneratePath()
    {/*
        path = transform.GetComponent<PathCreator>().path;
        ClearPath();
        for (var i = 0; i < path.NumPoints; i++)
        {
            if (i % 3 != 0) continue;

            var tempJumpingPad = Instantiate(JumpingPadPrefab, transform, true);
            tempJumpingPad.transform.position = path.localPoints[i];
        }*/
    }

    private void ClearPath()
    {
        foreach (Transform child in transform) {
            DestroyImmediate(child.gameObject);
        }
    }
}
