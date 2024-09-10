using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int _maxLife;
    private int _currentLife;

    private bool _invincible;

    public void TakeDamage()
    {
        _currentLife -= 1;
        _invincible = true;
        StartCoroutine(HitStun());

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
    }

    public IEnumerator HitStun()
    {
        yield return new WaitForSeconds(.5f);
        _invincible = false;
    }
}
