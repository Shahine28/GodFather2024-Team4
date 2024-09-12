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

    public Animator playerAnimator;


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

        playerAnimator.SetInteger("VerticalMovement", _move.y == 0 ? 0 : _move.y > 0 ? 1 : -1);
    }
}
