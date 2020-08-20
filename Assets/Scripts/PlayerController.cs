﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;

    private Vector3 firstTouchForwardDirection;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.GameState != GameConstants.GameState.Playable) return;
        var isFirstTouch = InputManager.Instance.InputState == GameConstants.InputState.FirstTouch;
        
        if (InputManager.Instance.InputState == GameConstants.InputState.Hold || isFirstTouch)
        {
            if (isFirstTouch)
            {
                firstTouchForwardDirection = transform.forward;
            }

            HandleRotation();
            
            HandleMovement();
        }
        
        
        
    }

    public void HandleRotation()
    {
        var angleDifference = (InputManager.Instance.LastTouchPosition.x - InputManager.Instance.FirstTouchPosition.x) / 4f;
        
        transform.forward = Quaternion.Euler(0, angleDifference, 0) * firstTouchForwardDirection;
    }

    public void HandleMovement()
    {
        transform.position += transform.forward * movementSpeed;    // Forward movement while holding.
    }
}