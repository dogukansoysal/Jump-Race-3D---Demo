using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class JumpingPad : MonoBehaviour
{
    public int Health = 4;
    public float BounceMagnitude;
    public int Index;
    
    public int BreakingPartCount;
    public List<Transform> BreakableParts;

    public void Start()
    {
        BreakingPartCount = BreakableParts.Count / (Health - 1);
    }

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

    public void HandleHealth()
    {
        if (GameManager.Instance.GameState != GameConstants.GameState.Playable) return;

        if (Health <= 0)
        {
            transform.GetComponent<MeshCollider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).GetComponent<MeshCollider>().enabled = false;

            for (int i = 0; i < transform.GetChild(2).childCount; i++)
            {
                var breakableRb = transform.GetChild(2).GetChild(i).GetComponent<Rigidbody>();
                breakableRb.isKinematic = false;
                if(Random.Range(0,10) == 0)
                    breakableRb.AddExplosionForce(500f, transform.position + (-transform.up * 3), 50f);
                else
                {
                    breakableRb.AddExplosionForce(1000f, transform.position + (transform.up * 5), 50f);
                }
                //childRb.AddForce(Vector3.down * 500);
            }
            
            Destroy(gameObject, 5f);
        }
        else
        {
            for (var i = 0; i < BreakingPartCount; i++)
            {
                var breakableIndex = Random.Range(0, BreakableParts.Count);
                var breakable = BreakableParts[breakableIndex];
                breakable.SetParent(null);
                breakable.GetComponent<Rigidbody>().isKinematic = false;
                breakable.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position + (-transform.up * 7), 50f);
                breakable.GetComponent<Rigidbody>().AddForce(Vector3.down * 300);
                Destroy(breakable.gameObject, 5f);
                BreakableParts.RemoveAt(breakableIndex);
            }
            
        }
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            if (GameManager.Instance.GameState == GameConstants.GameState.Playable) // Check if game is playable
            {
                Health--;
            }
                
            Bounce(collider);
            collider.transform.GetComponent<PlayerController>().HandleBounce();
        }
        else if (collider.transform.CompareTag("Enemy"))
        {
            if(GameManager.Instance.GameState == GameConstants.GameState.Playable)    // Check if game is playable
            {
                Health--;
            }   
                
            Bounce(collider);
            collider.transform.GetComponent<EnemyAI>().HandleBounce();

        }

        HandleHealth();
    }
}
