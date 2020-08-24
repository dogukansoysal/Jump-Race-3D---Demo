using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public float MaxMovementSpeed;

    public float FallMultiplier;
    
    private Vector3 firstTouchForwardDirection;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private float currentMovementSpeed;
    private void Awake()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        _animator = transform.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BetterGravity();

        if(GameManager.Instance.GameState != GameConstants.GameState.Playable) return;
        
        if (InputManager.Instance.InputState == GameConstants.InputState.Hold || InputManager.Instance.InputState == GameConstants.InputState.FirstTouch)
        {
            if (firstTouchForwardDirection == Vector3.zero)
            {
                firstTouchForwardDirection = transform.forward;
            }
            else
            {
                HandleRotation();
            }

            currentMovementSpeed += 60f * Time.deltaTime;
            currentMovementSpeed = Mathf.Clamp(currentMovementSpeed, 0f, MaxMovementSpeed);
            
            HandleMovement();
        }
        else if (InputManager.Instance.InputState == GameConstants.InputState.Released || InputManager.Instance.InputState == GameConstants.InputState.None)
        {
            ResetVelocity();
        }
        
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
    }

    private void HandleRotation()
    {
        if(firstTouchForwardDirection == Vector3.zero) return;
        
        var angleDifference = (InputManager.Instance.LastTouchPosition.x - InputManager.Instance.FirstTouchPosition.x) / 5f;
        
        transform.forward = Quaternion.Euler(0, angleDifference, 0) * firstTouchForwardDirection;
    }

    private void HandleMovement()
    {
        //transform.position += transform.forward * movementSpeed;    // Forward movement while holding.
        
        _rigidbody.velocity = new Vector3(transform.forward.x * currentMovementSpeed, _rigidbody.velocity.y, transform.forward.z * currentMovementSpeed);
    }

    private void BetterGravity()
    {
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector3.up * Physics.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
        }
    }

    private void ResetVelocity()
    {
        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        _rigidbody.angularVelocity = Vector3.zero;
        currentMovementSpeed = 0;
        firstTouchForwardDirection = Vector3.zero;
    }
}
