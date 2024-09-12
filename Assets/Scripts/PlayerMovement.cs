using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    public float MaxSpeed => _maxSpeed; 

    private Vector2 _move;
    private float _bindingDirection = 1;

    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _playerMovement;
    public Vector2 PlayerMove => _playerMovement;

    public Animator playerAnimator;


    public void UpdateMoveDirection(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
        _playerMovement = _move.normalized;
    }

    void UpdateMoveSpeed()
    {
        _rb.velocity = new Vector2(_maxSpeed * _move.x * _bindingDirection, _maxSpeed * _move.y * _bindingDirection);
    }

    public void ReverseBindingDirection()
    {
        _bindingDirection *= -1;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateMoveSpeed();

        playerAnimator.SetInteger("VerticalMovement", _move.y == 0 ? 0 : _move.y > 0 ? 1 : -1);
    }

    public void SetMaxSpeed(float maxSpeed)
    {
        _maxSpeed = maxSpeed;
    }
}
