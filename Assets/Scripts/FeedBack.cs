
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class FeedBack : MonoBehaviour
{
    [SerializeField] private UnityEvent _onEnemyDeath;
    [SerializeField] private ParticleSystem _particleSystemEnemyDeath;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyDeath()
    {
        Debug.Log("Enemy is dead");
        _onEnemyDeath.Invoke();
    }

    public void OnEnemyPosition(Vector3 position)
    {
        _particleSystemEnemyDeath.transform.position = position;
        _particleSystemEnemyDeath.Play();
    }
}
