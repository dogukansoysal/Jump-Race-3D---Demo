﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SplineMesh;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
[SelectionBase]
[DisallowMultipleComponent]
public class PathGenerator : MonoBehaviour {
    private GameObject generated;
    private Spline spline = null;
    private bool toUpdate = true;

    public GameObject prefab = null;
    public float scaleRange = 0;
    public float spacing = 1, spacingRange = 0;
    public float offset = 0, offsetRange = 0;
    public bool isRandomYaw = false;
    public int randomSeed = 0;

    public List<Color> Colors;
    private void OnEnable() {
        string generatedName = "generated by "+GetType().Name;
        var generatedTranform = transform.Find(generatedName);
        generated = generatedTranform != null ? generatedTranform.gameObject : UOUtility.Create(generatedName, gameObject);
        
        spline = GetComponentInParent<Spline>();
        spline.NodeListChanged += (s, e) => {
            toUpdate = true;
            foreach (CubicBezierCurve curve in spline.GetCurves()) {
                curve.Changed.AddListener(() => toUpdate = true);
            }
        };
        foreach (CubicBezierCurve curve in spline.GetCurves()) {
            curve.Changed.AddListener(() => toUpdate = true);
        }
    }

    private void OnValidate() {
        toUpdate = true;
    }

    private void Update() {
        if (toUpdate) {
            Sow();
            toUpdate = false;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void Sow() {
        UOUtility.DestroyChildren(generated);

        UnityEngine.Random.InitState(randomSeed);
        if (spacing + spacingRange <= 0 ||
            prefab == null)
            return;

        float distance = 0;
        var sampleCount = (int)(spline.Length / spacing) + 1;
        while (distance <= spline.Length) {
            CurveSample sample = spline.GetSampleAtDistance(distance);

            GameObject go;
            go = Instantiate(prefab, generated.transform);
            go.transform.localRotation = Quaternion.identity;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            // move along spline, according to spacing + random
            go.transform.localPosition = sample.location;
            // apply scale + random
            Vector3 rangedScale = prefab.transform.localScale + (prefab.transform.localScale * UnityEngine.Random.Range(0, scaleRange));
            
            go.transform.localScale = rangedScale;
            /*
            // rotate with random yaw
            if (isRandomYaw) {
                go.transform.Rotate(0, 0, UnityEngine.Random.Range(-180, 180));
            } else {
                go.transform.rotation = sample.Rotation;
            }
            */
            // move orthogonaly to the spline, according to offset + random
            var binormal = (Quaternion.LookRotation(sample.tangent, sample.up) * Vector3.right).normalized;
            var localOffset = offset + UnityEngine.Random.Range(0, offsetRange * Math.Sign(offset));
            localOffset *=  sample.scale.x;
            binormal *= localOffset;
            go.transform.position += binormal;
            

            //go.GetComponent<Renderer>().material.color = Colors[Random.Range(0, Colors.Count)];
            
            if (distance - spacing > 0)
            {
                var heading = spline.GetSampleAtDistance(distance - spacing).location - go.transform.position;
                go.transform.forward = -(heading / heading.magnitude);
            }
            
            var textComponent = go.transform.GetChild(0).GetComponent<TextMeshPro>();

            if (textComponent)
            {
                if (sampleCount > 0)
                {
                    textComponent.text = sampleCount.ToString();
                    sampleCount--; 
                }
            }
            
            distance += spacing + UnityEngine.Random.Range(0, spacingRange);

        }
        
        //DrawPathLine();

    }

    /// <summary>
    /// A Guide Line for player to follow the current path
    /// </summary>
    public void DrawPathLineRenderer()
    {
        var lr = transform.GetComponent<LineRenderer>();
        lr.positionCount = generated.transform.childCount;
        for (var i = 0; i < lr.positionCount; i++)
        {
            lr.SetPosition(i, transform.GetChild(i).position);
        }
    }

    public void DrawPathLine()
    {
        
    }
    
}

