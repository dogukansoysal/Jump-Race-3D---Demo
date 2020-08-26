using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPad : MonoBehaviour
{
    public int Health;
    public float BounceMagnitude;
    public int Index;
    
    /// <summary>
    /// Generic bounce method for multiple purpose.
    /// </summary>
    /// TODO: Move this method to the GameConstants static class or make it static for future work.
    /// <param name="collision"></param>
    public void Bounce(Collider collider)
    {
        collider.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        collider.transform.GetComponent<Rigidbody>().AddForce( Vector3.up * BounceMagnitude,   
            ForceMode.Impulse);
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            Bounce(collider);
            collider.transform.GetComponent<PlayerController>().HandleBounce();
        }
        else if (collider.transform.CompareTag("Enemy"))
        {
            Bounce(collider);
            collider.transform.GetComponent<EnemyAI>().HandleBounce();

        }
    }
}
