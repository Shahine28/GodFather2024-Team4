using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CircleCollider2D _coll;
    [SerializeField] private PlayerLife _lifeScript;
    [SerializeField] private float cooldownTime;
    private float recharge;

    void StartAttack()
    {
        _coll.radius = 1.5f;
        _coll.isTrigger = true;
        _animator.SetBool("Attacking", true);
        recharge = 0;
        _animator.SetBool("Attacking", false);
        StartCoroutine(AttackTime());
        StartCoroutine(_lifeScript.HitStun());
    }

    void AttackUpdate()
    {
        if (recharge < cooldownTime)
        {
            recharge += Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && recharge >= cooldownTime)
        {
            StartAttack();
        }
    }

    private void Start()
    {
        recharge = cooldownTime;
    }

    void Update()
    {
        AttackUpdate();
    }

    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(.5f);
        _coll.radius = 0.5f;
        _coll.isTrigger = false;
    }
}
