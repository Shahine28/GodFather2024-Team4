
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private  InputManager _inputManager;
    [SerializeField] private PlayerMovement _playerMovement;

    [SerializeField] private bool _isUsingKeyboard => _inputManager.IsUsingGamepad()== false;
    //[SerializeField] private Animator _animator;
    [Header("Attack")]
    [SerializeField] private PolygonCollider2D _coll;
    [SerializeField] private float _cooldownTime;
    private float _reloadTime;
    [SerializeField] private float _damage;
    public float Damage => _damage;
    [SerializeField] private ContactFilter2D _contactFilter;
    [SerializeField] private List<Collider2D> _collidersInContact = new List<Collider2D>();

    [Header("Rotation")]
    [SerializeField] private Transform parentObject; // Référence au parent autour duquel tourner
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform _playerTransform;

    [Header("Sprite")]
    [SerializeField] private GameObject _sprite;

    private Vector2 lookInput; // Stocke l'input de la souris ou du joystick

    private void Start()
    {
        mainCamera = Camera.main;
        _sprite.SetActive(false);
    }

    private void Update()
    {
        if (_reloadTime < _cooldownTime)
        {
            _reloadTime += Time.deltaTime;
        }
        RotateChildAroundParent();
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        if (context.performed) lookInput = context.ReadValue<Vector2>();
        if (context.canceled && !_isUsingKeyboard) lookInput = Vector2.zero;
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
        // Si l'input utilise le clavier, on prend en compte la souris
        else if (_isUsingKeyboard)
        {
            Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            direction = (worldMousePos - parentObject.position).normalized; // Calcul de la direction vers la souris
        }
        // Si aucun input n'est actif, on utilise la direction du mouvement
        else if (_playerMovement.PlayerMove.magnitude > 0 && !_isUsingKeyboard)
        {
            direction = _playerMovement.PlayerMove.normalized; // Orienter vers la direction du mouvement
        }
        else
        {
            direction = Vector2.zero; // Aucun input ou mouvement, aucune rotation
        }

        // Calcul de l'angle pour orienter l'objet enfant sans changer sa position
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            // Appliquer la rotation à l'enfant sans changer sa position
            transform.localRotation = rotation; // On garde la position initiale relative au parent
        }
    }



    public void Attack(InputAction.CallbackContext context)
    {      
        if (context.performed && _reloadTime >= _cooldownTime)
        {
            Debug.Log("Attack");
            _sprite.SetActive(true);
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
            StartCoroutine(enumerator());
        }
        else if (context.canceled && _sprite.activeInHierarchy)
        {
            _sprite.SetActive(false);
        }
    }
    
    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(0.1f);
        if (_sprite.activeInHierarchy)
        {
            _sprite.SetActive(false);
        }

    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }


   
}
