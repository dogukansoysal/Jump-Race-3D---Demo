using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [Tooltip("1 is Hardest, 0 is Easiest")]
    public float DifficultyRatio;
    public float MovementSpeed;
    public GameObject Target;
    
    public bool isBounced = false;    // Check if AI jumped on target JumpingPad.
    
    private Rigidbody _rigidbody;
    private RaycastHit hit;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        _animator = transform.GetComponent<Animator>();
    }

    private void Start()
    {
        Target = PathGenerator.Instance.JumpingPads[0];
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            if (hit.collider.CompareTag("JumpingPad"))
            {
                if (Target == hit.transform.gameObject)
                {
                    ResetVelocity();
                    if (isBounced)
                    {
                        ChangeTarget();
                    }
                }
                else
                {
                    HandleMovement();
                }
            }
            else
            {
                HandleMovement();
            }
        }
        else
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        _rigidbody.velocity = new Vector3(transform.forward.x * MovementSpeed, _rigidbody.velocity.y, transform.forward.z * MovementSpeed);
    }

    private void HandleRotation()
    {
        transform.LookAt(new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z));
    }

    public void HandleBounce()
    {
        if (Random.Range(0, 2) % 2 == 0)
        {
            _animator.SetTrigger("Jump");
        }
        else
        {
            _animator.SetTrigger("Backflip");
        }
        
        if (Target == hit.transform.gameObject)
        {
            isBounced = true;
        }
    }
    
    
    private void ChangeTarget()
    {
        var jumpingPadIndex = Target.GetComponent<JumpingPad>().Index;
        if (jumpingPadIndex < PathGenerator.Instance.JumpingPads.Count - 1)
        {
            Target = PathGenerator.Instance.JumpingPads[jumpingPadIndex + 1];
        }
        
        isBounced = false;
        
        ResetVelocity();
        HandleRotation();
    }
    
    private void ResetVelocity()
    {
        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
