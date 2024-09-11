using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int _maxLife;
    [SerializeField] private SpriteRenderer _skin;
    private HealthBar _healthBarScript;
    private int _currentLife;

    private bool _invincible;

    public void TakeDamage()
    {
        if (!_invincible)
        {
            _currentLife -= 1;
            _invincible = true;
            _healthBarScript.Wound();
            StartCoroutine(HitStun());
            StartCoroutine(Flash());
        }
        
        if (_currentLife == 0 )
        {
            GameOver();
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
