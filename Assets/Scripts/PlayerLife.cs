
using MoreMountains.Feedbacks;
using System.Collections;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private float _maxLife;
    public float MaxLife => _maxLife;
    [SerializeField] private float _currentLife;
    public float CurrLife => _currentLife;
    public float CurrentLife => _currentLife;
    [SerializeField] private Slider _healthBar;

    [SerializeField] private MMF_Player _damageFeedback;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private TextMeshProUGUI _finalScore;
    private bool _invincible;
    [SerializeField] private float _invincibleTime = 0.25f;
    void Start()
    {
        if (_waveManager == null) _waveManager = FindObjectOfType<WaveManager>();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        _currentLife = _maxLife;
        _healthBar.maxValue = _maxLife;
        _healthBar.value = _maxLife;
    }
    public void TakeDamage(float Damage)
    {
        if (_invincible) return;
        _currentLife -= Mathf.Abs(Damage);
        _invincible = true;
        UpdateHealthBar();
        if (_currentLife <= 0)
        {
            _damageFeedback.StopFeedbacks();
            GameOver();
        }  
        else
        {
            _damageFeedback.PlayFeedbacks();
            StartCoroutine(HitStun());
        }

    }

    IEnumerator HitStun()
    {
        yield return new WaitForSeconds(_invincibleTime);
        _invincible = false;
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
        _finalScore.text = "NOMBRE DE VICTIMES: " + _waveManager.WaveText.text;
        _gameOverScreen.SetActive(true);
        Time.timeScale = 0;
        //#if UNITY_EDITOR
        //UnityEditor.EditorApplication.isPlaying = false;
        //#endif
        //Application.Quit();
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
