using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    
    private float _moveX;
    private float _moveY;

    [SerializeField] private Rigidbody2D _rb;

    private bool _isMoving => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

    void UpdateMoveDirection()
    {
        _moveX = Input.GetAxis("Horizontal");
        _moveY = Input.GetAxis("Vertical");

    }

    void UpdateMoveSpeed()
    {
        UpdateMoveDirection();
        _rb.velocity = new Vector2(_maxSpeed * _moveX, _maxSpeed * _moveY);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveSpeed();
    }
}
