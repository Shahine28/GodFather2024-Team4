using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    

    [SerializeField] private WaveManager _waveManager;
    
    private GameObject _player;
    [Header("Movement")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _stopDistance = 1f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Attack")]
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _attackSpeed = 1;
    private float _attackTimer = 0;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private bool _IsInAttackRange = false;

    [Header("Health")]
    [SerializeField] private float _health = 100;
    [SerializeField] private Slider _healthBar;
    private Canvas _canvas;

    // Start is called before the first frame update
    void Start()
    {
        // On trouve le joueur via son tag
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");
        if (_waveManager == null) _waveManager = GameObject.FindObjectOfType<WaveManager>();
        if (_healthBar == null) _healthBar = GetComponentInChildren<Slider>();
        if (_canvas == null) _canvas = GetComponentInChildren<Canvas>();

    }

    // Update is called once per frame
    void Update()
    {
        _canvas.transform.rotation = Quaternion.identity;
        RotateTowardPlayer();
        if (CanMove()) MoveTowardPlayer();
        else _rb.velocity = Vector2.zero;
        Damage(_damage);
    }

    public void SetEnemy(float health,float speed, float damage, float attackSpeed, GameObject player, WaveManager waveManager)
    {
        _health = health;
        _healthBar.maxValue = _health;
        _healthBar.value = _health;
        _speed = speed;
        _damage = damage;
        _attackSpeed = attackSpeed;
        _player = player;
        _waveManager = waveManager;
    }

    public void SetEnemy(float health, float speed, float damage, float attackSpeed, GameObject player, WaveManager waveManager, float RotationSpeed, float attackRange)
    {
        _health = health;
        _healthBar.maxValue = _health;
        _healthBar.value = _health;
        _speed = speed;
        _damage = damage;
        _attackSpeed = attackSpeed;
        _player = player;
        _waveManager = waveManager;
        _rotationSpeed = RotationSpeed;
        _attackRange = attackRange;
    }
    private void RotateTowardPlayer()
    {
        if (_player != null)
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private bool CanMove()
    {

        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, _stopDistance, enemyLayer);
        if (nearbyEnemies.Length > 1)  
        {
            return false;
        }
        return true;
    }

    private void MoveTowardPlayer()
    {
        if (_player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
            if (distanceToPlayer - _attackRange / 2 > _attackRange) //On vérifie si le joueur ou un autre enemy est dans la range d'attaque
            {
                Vector2 direction = (_player.transform.position - transform.position).normalized;
                _rb.velocity = direction * _speed;
                if (_IsInAttackRange) _IsInAttackRange = false; _attackTimer = _attackSpeed;
            }
            else
            {
                _rb.velocity = Vector2.zero;
                if (!_IsInAttackRange) _IsInAttackRange = true;
            }
        }
    }

    [Button]
    public void TakeDamage() //C'est pour tester la fonction TakeDamage dans l'éditeur
    { TakeDamage(10); }
    public void TakeDamage(float damage)
    {
        _health -= Mathf.Abs(damage);
        _healthBar.value = _health;
        if (_health <= 0)
        {
            _waveManager.StartCoroutine(_waveManager.RemoveEnemy(gameObject));
            Destroy(gameObject);
        }
    }

    private void Damage(float damage)
    {
        _attackTimer -= Time.deltaTime;
        if (_player != null && _IsInAttackRange && _attackTimer <= 0)
        {
            //PlayerLife player = _player.GetComponent<PlayerLife>();
            //player.TakeDamage(damage);
            Debug.Log("Player took " + damage + " damage");
            _attackTimer = _attackSpeed;
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _stopDistance);
    }

    
}



