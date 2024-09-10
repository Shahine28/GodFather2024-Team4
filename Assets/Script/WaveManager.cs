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

    [Header("Enemy Attack")]
    [SerializeField] private float _baseDamage = 10;
    [SerializeField] private float _damagePerWave = 1f;
    [SerializeField] private float _baseAttackSpeed = 1f;
    [SerializeField] private float _attackSpeedPerWave = 0.1f;

    private List<GameObject> _enemies = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(StartWave());
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator StartWave()
    {
        _waveNumber++;
        for (int i = 0; i < _enemiesPerWave; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition();
            //while (IsPositionOccupied(spawnPosition))
            //{
            //    spawnPosition = GetSpawnPosition();
            //}
            GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetEnemy(_baseHealth + _healthPerWave * _waveNumber, _baseSpeed + _speedPerWave * _waveNumber,
                _baseDamage + _damagePerWave * _waveNumber, _baseAttackSpeed + _attackSpeedPerWave * _waveNumber, _player, this);

            _enemies.Add(enemy);
            yield return new WaitForSeconds(_timeBetweenEnemies);
        }
    }

    public void NextWave()
    {
        Debug.Log("Starting wave " + (_waveNumber + 1));
        _enemiesPerWave += _enemiesPerWaveIncrement;
        StartCoroutine(StartWave());
    }

    // Génère une position en dehors du champ de vision de la caméra
    private Vector3 GetSpawnPosition()
    {
        Camera cam = Camera.GetComponent<Camera>();
        Vector3 cameraPosition = cam.transform.position;
        float spawnDistance = Random.Range(5f, 20f);
        Vector3 spawnDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = cameraPosition + spawnDirection * spawnDistance;
        spawnPosition.z = 0;
        return spawnPosition;
    }
    private bool IsPositionOccupied(Vector3 position)
    {
        float minDistance = 1.5f;
        foreach (GameObject enemy in _enemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) < minDistance)
            {
                  return true;
            }
        }
        return false;
    }

    public IEnumerator RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
        if (_enemies.Count == 0)
        {
            Debug.Log("Wave " + _waveNumber + " cleared!");
            yield return new WaitForSeconds(_timeBetweenWaves);
            NextWave();
        }
        yield return null;
    }
}

