using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private float _maxLife;
    [SerializeField] private float _currentLife;
    //private bool _invincible;
    void Start()
    {
        _currentLife = _maxLife;
    }
    public void TakeDamage(float Damage)
    {
        _currentLife -= Mathf.Abs(Damage);
        if (_currentLife <= 0)
        {
            GameOver();
        }

        //if (!_invincible)
        //{

        //_invincible = true;
        //StartCoroutine(HitStun());
        //}
    }

    public void Heal(float Heal)
    {
        _currentLife += Mathf.Abs(Heal);
        if (_currentLife > _maxLife)
        {
            _currentLife = _maxLife;
        }
    }

    private void GameOver()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    void Start()
    {
        _currentLife = _maxLife;
        _healthBarScript = FindObjectOfType<HealthBar>();
    }


    //public IEnumerator HitStun()
    //{
    //    yield return new WaitForSeconds(.5f);
    //    _invincible = false;
    //}
    public IEnumerator HitStun()
    {
        yield return new WaitForSeconds(.5f);
        _invincible = false;
    }

    public IEnumerator Flash()
    {
        yield return new WaitForSeconds(.10f);
        _skin.color = Color.clear;
        yield return new WaitForSeconds(.10f);
        _skin.color = Color.white;
        yield return new WaitForSeconds(.10f);
        _skin.color = Color.clear;
        yield return new WaitForSeconds(.10f);
        _skin.color = Color.white;
        yield return new WaitForSeconds(.10f);
        _skin.color = Color.clear;
        yield return new WaitForSeconds(.10f);
        _skin.color = Color.white;
    }
}
