using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;

    private Vector2 _move;

    [SerializeField] private Rigidbody2D _rb;

    private bool _isMoving => Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

    public void UpdateMoveDirection(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();

    }

    void UpdateMoveSpeed()
    {
        _rb.velocity = new Vector2(_maxSpeed * _move.x, _maxSpeed * _move.y);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveSpeed();
    }
}
