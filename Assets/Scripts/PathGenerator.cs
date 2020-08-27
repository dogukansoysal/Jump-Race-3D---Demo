using System;
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
public class PathGenerator : MonoBehaviour
{
    public static PathGenerator Instance;
    
    private GameObject generated;
    private Spline spline = null;
    private bool toUpdate = true;

    public GameObject prefab = null;
    public float scaleRange = 0;
    public float spacing = 1, spacingRange = 0;
    public float offset = 0, offsetRange = 0;
    public bool isRandomYaw = false;
    public int randomSeed = 0;

    [Tooltip("If true, the mesh will be bent on play mode. If false, the bent mesh will be kept from the editor mode, allowing lighting baking.")]
    public bool updateInPlayMode;
    
    public List<Material> Materials;

    public List<GameObject> JumpingPads;
    public GameObject FinishPad;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void OnEnable() {
        string generatedName = "JumpingPads";
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
        if (!updateInPlayMode && Application.isPlaying) return;

        if (toUpdate) {
            Sow();
            toUpdate = false;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void Sow() {
        JumpingPads = new List<GameObject>();
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
            
            go.transform.GetComponent<JumpingPad>().Index = JumpingPads.Count;

            var textComponent = go.transform.GetChild(0).GetComponent<TextMeshPro>();
            if (textComponent)
            {
                if (sampleCount > 0)
                {
                    textComponent.text = sampleCount.ToString();
                    sampleCount--; 
                }
            }
            
            go.GetComponent<Renderer>().sharedMaterial = Materials[Random.Range(0, Materials.Count)];
            
            if (distance - spacing > 0)
            {
                var targetPos = spline.GetSampleAtDistance(distance - spacing).location;
                targetPos.y = go.transform.position.y;
                
                var heading = targetPos - go.transform.position;
                go.transform.forward = -(heading / heading.magnitude);
            }

            distance += spacing + UnityEngine.Random.Range(0, spacingRange);
            
            JumpingPads.Add(go);
        }
        
        SetFinishZone();
        
        //DrawPathLine();
    }

    /// <summary>
    /// Last jumping pad will be changed as finish zone.
    /// </summary>
    private void SetFinishZone()
    {
        FinishPad = JumpingPads[JumpingPads.Count - 1];
        DestroyImmediate(FinishPad.GetComponent<JumpingPad>());
        FinishPad.transform.GetChild(0).GetComponent<TextMeshPro>().text = "";
        FinishPad.transform.GetChild(1).tag = "Finish";
        FinishPad.transform.localScale = new Vector3(FinishPad.transform.localScale.x * 2f, FinishPad.transform.localScale.y, FinishPad.transform.localScale.z * 2f);
        
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

