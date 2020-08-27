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
            transform.GetComponent<MeshCollider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).GetComponent<MeshCollider>().enabled = false;

            for (int i = 0; i < transform.GetChild(2).childCount; i++)
            {
                var childRb = transform.GetChild(2).GetChild(i).gameObject.AddComponent<Rigidbody>();
                childRb.AddExplosionForce(1000f, transform.position + (transform.up * 10), 50f);
                //childRb.AddForce(Vector3.down * 500);
            }
            
            Destroy(gameObject, 5f);
            return;
        }
        
        if (Health - 1 < BreakableModels.Count)
        {
            if (transform.childCount > 2)
            {
                Destroy(transform.GetChild(2).gameObject);
            }
            transform.GetComponent<MeshRenderer>().enabled = false;
            transform.GetComponent<MeshFilter>().mesh = null;
            
            var newModel = Instantiate(BreakableModels[Health - 1], transform);
            newModel.transform.localScale = new Vector3(0.01f,0.1f,0.01f);

            if (Health - 1 > 0)
            {
                newModel.transform.localPosition = Vector3.zero + new Vector3(0,0,-1);
                newModel.GetComponent<MeshRenderer>().sharedMaterial = transform.GetComponent<MeshRenderer>().material;
            }
            else
            {
                newModel.transform.localPosition = Vector3.zero + new Vector3(0,0,0);
                for (int i = 0; i < newModel.transform.childCount; i++)
                {
                    newModel.transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial = transform.GetComponent<MeshRenderer>().material;
                }
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
