using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] private float _enemySize = 1f;
    [SerializeField] private Vector2 _randomRange;


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
            int occurence = 0;
            while (!IsPositionValid(spawnPosition) && occurence < _enemiesPerWave * _waveNumber)
            {
                occurence++;
                spawnPosition = GetSpawnPosition();
            }
            GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponentInChildren<Enemy>().SetEnemy(_baseHealth + _healthPerWave * _waveNumber, _baseSpeed + _speedPerWave * _waveNumber,
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

    // G�n�re une position en dehors du champ de vision de la cam�ra
    private Vector3 GetSpawnPosition()
    {
        Camera cam = Camera.GetComponent<Camera>();
        Vector3 cameraPosition = cam.transform.position;
        float spawnDistance = Random.Range(_randomRange.x, _randomRange.y);
        Vector3 spawnDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = cameraPosition + spawnDirection * spawnDistance;
        spawnPosition.z = 0;
        return spawnPosition;
    }
    private bool IsPositionValid(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, _enemySize);
        if (hit == null)
        {
            return true;
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

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Camera.transform.position, _enemySize);
    }
}

