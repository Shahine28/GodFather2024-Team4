using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    [SerializeField] private WaveManager _waveManager;
    
    private GameObject _player;
    [Header("Movement")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _rotationSpeed = 5f;

    [Header("Attack")]
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _attackSpeed = 1;
    private float _attackTimer = 0;
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private bool _IsInAttackRange = false;

    // Start is called before the first frame update
    void Start()
    {
        // On trouve le joueur via son tag
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardPlayer();
        MoveTowardPlayer();
        Damage(_damage);
    }

    public void SetEnemy(float health,float speed, float damage, float attackSpeed, GameObject player, WaveManager waveManager)
    {
        _health = health;
        _speed = speed;
        _damage = damage;
        _attackSpeed = attackSpeed;
        _player = player;
        _waveManager = waveManager;
    }

    public void SetEnemy(float health, float speed, float damage, float attackSpeed, GameObject player, WaveManager waveManager, float RotationSpeed, float attackRange)
    {
        _health = health;
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

    private void MoveTowardPlayer()
    {
        if (_player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
            if (distanceToPlayer - _attackRange / 2 > _attackRange)
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
    public void TakeDamage()
    { TakeDamage(10); }
    public void TakeDamage(float damage)
    {
        _health -= Mathf.Abs(damage);
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
    }
}



