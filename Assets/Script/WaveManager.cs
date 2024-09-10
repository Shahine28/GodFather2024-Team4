using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _player;

    [Header("Wave Settings")]
    [SerializeField] private int _waveNumber = 0;
    [SerializeField] private int _enemiesPerWave = 5;
    [SerializeField] private int _enemiesPerWaveIncrement = 1;
    [SerializeField] private float _timeBetweenWaves = 5f;
    [SerializeField] private float _timeBetweenEnemies = 1f;

    [Header("Enemy Settings")]    
    [SerializeField] private float _baseHealth = 100f;
    [SerializeField] private float _healthPerWave = 10f;
    
    [Header("Enemy Movement")]
    [SerializeField] private float _baseSpeed = 10f;
    [SerializeField] private float _speedPerWave = 1f;
    //[SerializeField] private float _baseRotationSpeed = 5f;

    [Header("Enemy Attack")]
    [SerializeField] private float _baseDamage = 10;
    [SerializeField] private float _damagePerWave = 1f;
    [SerializeField] private float _baseAttackSpeed = 1f;
    [SerializeField] private float _attackSpeedPerWave = 0.1f;
    //[SerializeField] private float _baseAttackRange = 1f;

    private List<Enemy> _enemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
