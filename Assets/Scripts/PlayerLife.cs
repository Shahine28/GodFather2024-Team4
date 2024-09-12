using Cinemachine.PostFX;
using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [Header ("Parameters")]
    [SerializeField] private float _maxLife;
    public float MaxLife => _maxLife;
    [SerializeField] private float _currentLife;
    public float CurrLife => _currentLife;
    public float CurrentLife => _currentLife;

    [Header ("Feedbacks")]
    [SerializeField] private MMF_Player _damageFeedback;

    [Header("UI")]
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private Slider _healthBar;
    //private bool _invincible;
    void Start()
    {
        _currentLife = _maxLife;
        _healthBar.maxValue = _maxLife;
        _healthBar.value = _maxLife;
    }
    public void TakeDamage(float Damage)
    {
        _currentLife -= Mathf.Abs(Damage);
        UpdateHealthBar();

        if (_currentLife <= 0)
        {
            GameOver();
        }  
        else
        {
            _damageFeedback.PlayFeedbacks();
        }
    }

    public void Heal(float Heal)
    {
        _currentLife += Mathf.Abs(Heal);
        if (_currentLife > _maxLife)
        {
            _currentLife = _maxLife;
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        _healthBar.value = _currentLife;
    }

    private void GameOver()
    {
        _gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void SetCurrentLife(float life)
    {
        _currentLife = life;
        _healthBar.value = _currentLife;
    }

    //public IEnumerator HitStun()
    //{
    //    yield return new WaitForSeconds(.5f);
    //    _invincible = false;
    //}
}
