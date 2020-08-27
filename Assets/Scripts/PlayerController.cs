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

        if(GameManager.Instance.GameState != GameConstants.GameState.Playable) return;    // Check if game is playable

        // Check if the player's altitude is lower than the last jumping pad's altitude.
        if (transform.position.y < PathGenerator.Instance.FinishPad.transform.position.y)
        {
            GameManager.Instance.Fail();
        }
        
        // Control the Input state as touching or not
        if (InputManager.Instance.InputState == GameConstants.InputState.Hold || InputManager.Instance.InputState == GameConstants.InputState.FirstTouch)
        {
            // If it's the first touch since user released, set the current direction as new forward direction to make rotations according to that direction.
            if (firstTouchForwardDirection == Vector3.zero)
            {
                firstTouchForwardDirection = transform.forward;
            }
            else
            {
                HandleRotation();
            }

            // 60f is arbitrary acceleration value.
            // TODO: Define acceleration variable.
            currentMovementSpeed += 60f * Time.deltaTime;
            currentMovementSpeed = Mathf.Clamp(currentMovementSpeed, 0f, MaxMovementSpeed);
            
            HandleMovement();
        }
        else if (InputManager.Instance.InputState == GameConstants.InputState.Released || InputManager.Instance.InputState == GameConstants.InputState.None)
        {
            ResetVelocity();
        }
        
    }

    /// <summary>
    /// Event Handler for the player when a bounce happens.
    /// </summary>
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
        
        var angleDifference = (InputManager.Instance.LastTouchPosition.x - InputManager.Instance.FirstTouchPosition.x) / 3f;
        
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


    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.Instance.GameState != GameConstants.GameState.Playable) return;

        if (other.CompareTag("Finish"))
        {
            GameManager.Instance.Success();
        }
    }
}
