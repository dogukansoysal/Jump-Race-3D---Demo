using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPad : MonoBehaviour
{
    public int Health;
    public float BounceMagnitude;
    
    /// <summary>
    /// Generic bounce method for multiple purpose.
    /// </summary>
    /// TODO: Move this method to the GameConstants static class or make it static for future work.
    /// <param name="collision"></param>
    public void Bounce(Collision collision)
    {
        collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        collision.transform.GetComponent<Rigidbody>().AddForce(Vector3.Reflect(Vector3.down, 
                collision.contacts[0].normal) * BounceMagnitude,   
            ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Bounce(other);
        }
    }
}
