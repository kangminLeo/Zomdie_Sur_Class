using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamage // IDamage�� �ڽ�
{
    // ��� ����ü���� ���� ������Ʈ
    /*
      ��üü��
      ����ü��
      �׾����� ��Ҵ��� -> �̺�Ʈ��
     */

    public float StartHealth = 100f;
    public float CurrentHealth { get; protected set; } // �θ𿡰Ը� ������ �� �ְ�
    public bool isDead { get; protected set; }
    public event Action onDead; // �׻� �տ� on�� ���δ�.

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
            //�״� �޼ҵ带 ȣ��
            Die();
        }
    }
    public virtual void Die() // �θ� virtual
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
