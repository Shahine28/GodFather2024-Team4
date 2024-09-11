using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _healthBarSlider;
    private PlayerLife _playerLifeScript;

    public void Wound()
    {
        _healthBarSlider.value -= 1;

    }

    public void Heal()
    {
        _healthBarSlider.value += 1;
    }

    void Start()
    {
        _healthBarSlider = GetComponent<Slider>();
        _playerLifeScript = FindObjectOfType<PlayerLife>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
