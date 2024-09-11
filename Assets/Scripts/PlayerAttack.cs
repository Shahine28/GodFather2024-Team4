using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //[SerializeField] private Animator _animator;
    [Header("Attack")]
    [SerializeField] private CircleCollider2D _coll;
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
    private float currentAngle = 0f; // Angle actuel pour la rotation

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
        lookInput = context.ReadValue<Vector2>();
    }

    private void RotateChildAroundParent()
    {
        Vector2 direction;

        // Si l'input du joystick est actif
        if (lookInput.magnitude > 0.1f)
        {
            direction = lookInput.normalized; // Normaliser l'input du stick pour obtenir la direction
        }
        else
        {
            // Si l'input de la souris est utilisé
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(mousePosition);
            direction = (worldMousePos - parentObject.position).normalized; // Calcul de la direction vers la souris
        }

        // Calcul de la nouvelle position de l'enfant autour du parent
        Vector3 newPosition = parentObject.position + new Vector3(direction.x, direction.y, 0) * _rotationRadius;

        // Appliquer instantanément la position de l'enfant
        transform.position = newPosition;
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
