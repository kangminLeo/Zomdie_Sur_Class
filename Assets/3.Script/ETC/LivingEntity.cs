using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamage // IDamage의 자식
{
    // 모든 생명체에게 붙일 컴포넌트
    /*
      전체체력
      현재체력
      죽었는지 살았는지 -> 이벤트로
     */

    public float StartHealth = 100f;
    public float CurrentHealth { get; protected set; } // 부모에게만 설정할 수 있게
    public bool isDead { get; protected set; }
    public event Action onDead; // 항상 앞에 on을 붙인다.

    protected virtual void OnEnable()
    {
        //
        isDead = false;
        CurrentHealth = StartHealth;
    }


    public virtual void OnDamage(float Damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        CurrentHealth -= Damage;
        // is dead or not?

        if(CurrentHealth <= 0 && !isDead)
        {
            //죽는 메소드를 호출
            Die();
        }
    }
    public virtual void Die() // 부모 virtual
    {
        if(onDead != null)
        {
            onDead();
        }
        isDead = true;
    }

    public virtual void Restore_health(float newHealth)
    {
        if(isDead)
        {
            return;
        }
        CurrentHealth += newHealth;
    }

}
