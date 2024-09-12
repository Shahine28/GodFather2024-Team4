using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private  InputManager _inputManager;

    [SerializeField] private bool _isUsingKeyboard => _inputManager.IsUsingGamepad()== false;
    //[SerializeField] private Animator _animator;
    [Header("Attack")]
    [SerializeField] private PolygonCollider2D _coll;
    [SerializeField] private float _cooldownTime;
    private float _reloadTime;
    [SerializeField] private float _damage;
    [SerializeField] private ContactFilter2D _contactFilter;
    [SerializeField] private List<Collider2D> _collidersInContact = new List<Collider2D>();

    [Header("Rotation")]
    [SerializeField] private Transform parentObject; // Référence au parent autour duquel tourner
    [SerializeField] private float _rotationSpeed = 5f; // Vitesse de rotation
    [SerializeField] private float _rotationRadius = 2f; // Rayon de rotation
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform _playerTransform;


    private Vector2 lookInput; // Stocke l'input de la souris ou du joystick

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (_reloadTime < _cooldownTime) _reloadTime += Time.deltaTime;
        
        RotateChildAroundParent();
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        /*if (context.performed) */lookInput = context.ReadValue<Vector2>();
        //if (context.canceled && !_isUsingKeyboard) lookInput = Vector2.zero;
    }

    private void RotateChildAroundParent()
    {
        Vector2 direction;

        // Si l'input du joystick est actif
        if (lookInput.magnitude > 0.65f && !_isUsingKeyboard)
        {
            Debug.Log("Joystick actif");
            direction = lookInput.normalized; // Normaliser l'input du stick pour obtenir la direction
        }
        else if (Mouse.current != null ||  _isUsingKeyboard) // Si l'input vient de la souris
        {
            Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = (worldMousePos - parentObject.position).normalized; // Calcul de la direction vers la souris
        }
        else // Si aucun input n'est actif
        {
            direction = Vector2.zero;
        }

        // Calcul de l'angle pour orienter l'objet enfant sans changer sa position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        // Appliquer la rotation à l'enfant pour qu'il pointe dans la bonne direction
        transform.rotation = rotation;
    }


    public void Attack(InputAction.CallbackContext context)
    {      
        if (context.performed && _reloadTime >= _cooldownTime)
        {
            Debug.Log("Attack");
            _collidersInContact.Clear();
            _coll.OverlapCollider(_contactFilter, _collidersInContact);
            foreach (var collider in _collidersInContact)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<Enemy>().TakeDamage(_damage);
                }
            }
            _reloadTime = 0;
        }
    }
   
}
