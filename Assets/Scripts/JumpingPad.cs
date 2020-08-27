using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPad : MonoBehaviour
{
    public int Health = 4;
    public float BounceMagnitude;
    public int Index;

    public List<GameObject> BreakableModels;

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
        if (Health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        
        if (Health - 1 < BreakableModels.Count)
        {
            if (transform.childCount > 2)
            {
                Destroy(transform.GetChild(2).gameObject);
            }
            var newModel = Instantiate(BreakableModels[Health - 1], transform);
            newModel.transform.localScale = new Vector3(0.01f,0.1f,0.01f);
            newModel.transform.localPosition = Vector3.zero + new Vector3(0,0,-1f);
            
            newModel.GetComponent<MeshRenderer>().sharedMaterial = transform.GetComponent<MeshRenderer>().material;
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetComponent<MeshFilter>().mesh = null;
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
