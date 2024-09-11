using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HappeningManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput _playerInput;

    [Header("Player")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private PlayerLife _playerLife;

    [Header("Happenings")]
    [SerializeField] private GameObject _waveManager;
    [SerializeField] private Vector3 _movementAttackLife;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bombe(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Bombe");
        }
    }
    public void Frenzie(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Frenzie");
        }
    }
    public void Multiplier(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Multiplier");
        }
    }

    public void Stats(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Stats");
        }
    }

    public void Binding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Binding");
        }
    }
    public void Vision(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Vision");
        }
    }





}
