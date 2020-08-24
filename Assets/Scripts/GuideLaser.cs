using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLaser : MonoBehaviour
{
    private LineRenderer lr;

    private RaycastHit hit;
    void Awake()
    {
        lr = transform.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            if (hit.collider.CompareTag("JumpingPad"))
            {
                lr.SetPosition(1,hit.point);
                lr.material.color = Color.green;

            }
            else
            {
                lr.SetPosition(1, -transform.up * 100);
                lr.material.color = Color.red;
            
            }
        }
        else
        {
            lr.SetPosition(1, -transform.up * 5000);
            lr.material.color = Color.red;
            
        }
    }
}
