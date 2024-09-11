using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HappeningManager : MonoBehaviour
{

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _waveManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buff(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Buff");
        }
    }
    public void Nerf(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Nerf");
        }
    }
}
