using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class HappeningManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private PlayerInput _playerInput;

    [Header("Player")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerAttack _playerAttack;
    [SerializeField] private PlayerLife _playerLife;

    [Header("Player Manager")]
    [SerializeField] private WaveManager _waveManager;

    [SerializeField, Foldout("Stats Nerf")] private Vector3 _movementAttackLifeNerf;
    [SerializeField, Foldout("Stats Nerf")] private float _statsNerfDuration = 10f;
    private bool _isStatsNerf = false;
    [SerializeField, Foldout("Stats Nerf")] private UnityEvent _statsNerfOnEvent;
    [SerializeField, Foldout("Stats Nerf")] private UnityEvent _statsNerfOffEvent;

    [SerializeField, Foldout("Score Multiplier")] private int _scoreMultiplierBuff = 2;
    [SerializeField, Foldout("Score Multiplier")] private float _multiplierBuffDuration = 10f;
    private bool _isMultiplier = false;
    [SerializeField, Foldout("Score Multiplier")] private UnityEvent _multiplierOnEvent;
    [SerializeField, Foldout("Score Multiplier")] private UnityEvent _multiplierOffEvent;

    [SerializeField, Foldout("Bombe")] private float _bombeDelayBewteenKills = 0.5f;
    [SerializeField, Foldout("Bombe")] private UnityEvent _bombeEvent;

    [SerializeField, Foldout("Reverse Binding")] private float _reverseBindingDuration = 10f;
    private bool _isReverseBinding = false;
    [SerializeField, Foldout("Reverse Binding")] private UnityEvent _reverseBindingOnEvent;
    [SerializeField, Foldout("Reverse Binding")] private UnityEvent _reverseBindingOffEvent;

    [SerializeField, Foldout("Vision")] private float _visionDuration = 10f;
    private bool _isVision = false;
    [SerializeField, Foldout("Vision")] private UnityEvent _visionOnEvent;
    [SerializeField, Foldout("Vision")] private UnityEvent _visionOffEvent;


    [SerializeField, Foldout("Heal")] private float _healingAmount = 10f;
    [SerializeField, Foldout("Heal")] private UnityEvent _healEvent;

    [SerializeField, Foldout("Frenzie")] private float _FrenzieDuration = 10f;
    [SerializeField, Foldout("Frenzie")] private Vector3 _movementAttackLifeBuff;
    [SerializeField, Foldout("Frenzie")] private UnityEvent _frenzieOnEvent;
    [SerializeField, Foldout("Frenzie")] private UnityEvent _frenzieOffEvent;

    private bool _isFrenzie = false;
    private float _maxSpeedFrenzie;
    private float _damageFrenzie;
    private float _currentLifeFrenzie;
    private float _maxSpeedNerf;
    private float _damageNerf;
    private float _currentLifeNerf;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bombe(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(_waveManager.Bombe(_bombeDelayBewteenKills));
            _bombeEvent?.Invoke();
            Debug.Log("Boooooooom !");
        }
    }

    public void Heal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Heal");
            _healEvent?.Invoke();
            _playerLife.Heal(_healingAmount);
        }
    }
    #region Frenzie
    public void Frenzie(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_isFrenzie) StartCoroutine(Frenzie());
            else FrenzieOff();
        }
    }
    private IEnumerator Frenzie()
    {
        _frenzieOnEvent?.Invoke();
        _isFrenzie = true;
        _maxSpeedFrenzie = _playerMovement.MaxSpeed;
        _damageFrenzie = _playerAttack.Damage;
        _currentLifeFrenzie = _playerLife.MaxLife;
        _playerMovement.SetMaxSpeed(_maxSpeedFrenzie + _movementAttackLifeBuff.x);
        _playerAttack.SetDamage(_damageFrenzie + _movementAttackLifeBuff.y);
        _playerLife.SetCurrentLife(_currentLifeFrenzie + _movementAttackLifeBuff.z);
        Debug.Log("Frenzie --> Stats Buffed for " + _statsNerfDuration + " seconds");
        yield return new WaitForSeconds(_statsNerfDuration);
        if (_isFrenzie) FrenzieOff();
    }
    
    private void FrenzieOff()
    {
        StopCoroutine(Frenzie());
        _frenzieOffEvent?.Invoke();
        _playerMovement.SetMaxSpeed(_maxSpeedFrenzie);
        _playerAttack.SetDamage(_damageFrenzie);
        _playerLife.SetCurrentLife(_currentLifeFrenzie);
        Debug.Log("Stats back to normal");
        _isFrenzie = false;
    }
    #endregion

    #region Multiplier
    public void Multiplier(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!_isMultiplier)  StartCoroutine(Multiplier());
            else MultiplierOff();
        }
    }
    private IEnumerator Multiplier()
    {
        _multiplierOnEvent?.Invoke();
        _isMultiplier = true;
        _waveManager.SetScoreMultiplier(_scoreMultiplierBuff);
        Debug.Log("Score Multiplier Buffed for " + _multiplierBuffDuration + " seconds");
        yield return new WaitForSeconds(_multiplierBuffDuration);
        if (_isMultiplier) MultiplierOff();
    }

    private void MultiplierOff()
    {
        _multiplierOffEvent?.Invoke();
        _isMultiplier = false;
        Debug.Log("Score Multiplier back to normal");
        _waveManager.SetScoreMultiplier(1);
    }
    #endregion

    #region Stats
    public void Stats(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_isStatsNerf) StartCoroutine(Stats());
            else StatsOff();
        }
    }
    private IEnumerator Stats()
    {
        _statsNerfOnEvent?.Invoke();
        _isStatsNerf = true;
        _maxSpeedNerf = _playerMovement.MaxSpeed;
        _damageNerf = _playerAttack.Damage;
        _currentLifeNerf = _playerLife.MaxLife;
        _playerMovement.SetMaxSpeed(_maxSpeedNerf - _movementAttackLifeNerf.x);
        _playerAttack.SetDamage(_damageNerf - _movementAttackLifeNerf.y);
        _playerLife.SetCurrentLife(_currentLifeNerf - _movementAttackLifeNerf.z);
        Debug.Log("Stats Nerfed for " + _statsNerfDuration + " seconds");
        yield return new WaitForSeconds(_statsNerfDuration);
        if (_isStatsNerf) StatsOff();
        
    }

    private void StatsOff()
    {
        _statsNerfOffEvent?.Invoke();
        _isStatsNerf = false;
        _playerMovement.SetMaxSpeed(_maxSpeedNerf);
        _playerAttack.SetDamage(_damageNerf);
        _playerLife.SetCurrentLife(_currentLifeNerf);
        Debug.Log("Stats back to normal");
    }
    #endregion

    #region Binding
    public void Binding(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_isReverseBinding) StartCoroutine(Binding());
            else BindingOff();
        }
    }

    private IEnumerator Binding()
    {
        _reverseBindingOnEvent?.Invoke();
        _isReverseBinding = true;
        _playerMovement.ReverseBindingDirection();
        Debug.Log("Binding reversed for " + _reverseBindingDuration + " seconds");
        yield return new WaitForSeconds(_reverseBindingDuration);
        if (_isReverseBinding) BindingOff();
       
    }

    private void BindingOff()
    {
        _reverseBindingOffEvent?.Invoke();
        _isReverseBinding = false;
        Debug.Log("Binding back to normal");
        _playerMovement.ReverseBindingDirection();
    }
    #endregion 

    #region Vision
    public void Vision(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_isVision) StartCoroutine(Vision());
            else VisionOff();
        }
    }
    private IEnumerator Vision()
    {
        _isVision = true;
        Debug.Log("Vision");
        _visionOnEvent?.Invoke();
        yield return new WaitForSeconds(0);
        if (_isVision) VisionOff();
    }

    private void VisionOff()
    {
       _isVision = false;
        _visionOffEvent?.Invoke();
        Debug.Log("Vision off");
    }
    #endregion
}
