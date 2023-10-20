using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ZombieController : LivingEntity
{
    [Header("������ ��� ���̾�")]
    public LayerMask TargetLayer;
    private LivingEntity TargetEntity;

    // ��θ� ����� AI Agent
    private NavMeshAgent agent;

    [Header("ȿ��")]
    [SerializeField] private AudioClip DeathClip;
    [SerializeField] private AudioClip HitClip;
    [SerializeField] private ParticleSystem HitEffect;

    private Animator Zombie_ani;
    private AudioSource Zombie_audio;

    /**/

    [Header("Info")]
    [SerializeField] private float Damage = 20f;
    [SerializeField] private float TimebetAttack = 0.5f;
    private float LastAttackTimebet;
    
    private bool isTarget
    {
        get
        {
            if(TargetEntity != null && !TargetEntity.isDead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        TryGetComponent(out agent);
        TryGetComponent(out Zombie_ani);
        TryGetComponent(out Zombie_audio);
    }

    public override void OnDamage(float Damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        /*
         ������ ����..
         �÷��̾����� �Ѿ��� �¾��� �� 
         �Ѿ��� ������ �ƾ� �Ҹ� ����� �ϰ�
         HitEffect-> ���� | �Ѿ��� ����� ����
         */
        if(!isDead)
        {
            HitEffect.transform.position = hitPosition;
            // hit ȸ������ �ٶ󺸴� ȸ���� ���·� ��ȯ
            HitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);

            HitEffect.Play();

            Zombie_audio.PlayOneShot(HitClip);
        }

        base.OnDamage(Damage, hitPosition, hitNormal);
    }

    public override void Die()
    {

        //�ݶ��̴� �ΰ� �־ �ϳ� �������

        base.Die();

        Collider[] colls = GetComponents<Collider>();

        foreach(Collider c in colls)
        {
            c.enabled = false; // ��Ȱ��ȭ
        }
        agent.isStopped = true;
        agent.enabled = false;
        Zombie_ani.SetTrigger("Die");
    }

    private void OnTriggerStay(Collider other)
    {
        // ��� ���� �� -> ���������� ȣ��
        /*
         Enter -> ��� ����
         Stay  -> ��� ���� ��
         Exit  -> ��� ���� ���� ��
         */
        if(!isDead && Time.time >= LastAttackTimebet + TimebetAttack)
        {
            if(other.TryGetComponent(out LivingEntity e))
            {
                if(TargetEntity.Equals(e))
                {
                    LastAttackTimebet = Time.time;
                    // ClosestPoint -> ��� ��ġ
                    // �� ���� �ǰ� ��ġ�� �ǰ� ������ �ٻ簪���� ����ϴ� �޼ҵ�

                    Vector3 hitPoint = other.ClosestPoint(transform.position);

                    Vector3 hitNormal = transform.position - other.transform.position;

                    e.OnDamage(Damage, hitPoint, hitNormal);
                }
            }
        }
        

    }

    private void Start()
    {
        StartCoroutine(Update_TargetPosition());
    }

    private void Update()
    {
        Zombie_ani.SetBool("HasTarget", isTarget);
    }



    private IEnumerator Update_TargetPosition()
    {
        while(!isDead)
        {
            if(isTarget)
            {
                agent.isStopped = false;
                agent.SetDestination(TargetEntity.transform.position);
            }
            else
            {
                agent.isStopped = true;
                Collider[] col = Physics.OverlapSphere(transform.position, 20f, TargetLayer);

                for(int i = 0; i < col.Length; i++)
                {
                    if(col[i].TryGetComponent(out LivingEntity e))
                    {
                        if(!e.isDead)
                        {
                            TargetEntity = e;
                            break;
                        }
                    }
                }
            }
            yield return null; // �� �����Ӿ�


        }
    }
}

