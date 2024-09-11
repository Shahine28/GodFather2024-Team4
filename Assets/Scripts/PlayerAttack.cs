using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PolygonCollider2D _coll;
    [SerializeField] private float _cooldownTime;
    private float _rechargeTime;

    void PointToMouse(Transform LookPoint) 
    {
        var mouseScreenPos = Input.mousePosition;
        var startingScreenPos = Camera.main.WorldToScreenPoint(LookPoint.position);
        mouseScreenPos.x -= startingScreenPos.x;
        mouseScreenPos.y -= startingScreenPos.y;
        var angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;
        LookPoint.rotation = Quaternion.Euler(new Vector3(0, 0, 90 + angle));
    }

    void StartAttack()
    {      
        StartCoroutine(AttackTime());
    }

    void AttackUpdate()
    {
        if (_rechargeTime < _cooldownTime)
        {
            _rechargeTime += Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && _rechargeTime >= _cooldownTime)
        {
            StartAttack();
        }
    }

    private void Start()
    {
        _rechargeTime = _cooldownTime;
        _coll.enabled = false;
    }

    void Update()
    {
        AttackUpdate();
        if (!_coll.enabled)
        {
            PointToMouse(transform);
        }
    }

    IEnumerator AttackTime()
    {
        _animator.SetBool("Attacking", true);
        _coll.enabled = true;

        yield return new WaitForSeconds(.5f);

        _animator.SetBool("Attacking", false);  
        _coll.enabled = false;
        _rechargeTime = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.SendMessage("TakeDamage");
    }
}
