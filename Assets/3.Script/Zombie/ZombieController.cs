using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ZombieController : LivingEntity
{
    [Header("추적할 대상 레이어")]
    public LayerMask TargetLayer;
    private LivingEntity TargetEntity;

    // 경로를 계산할 AI Agent
    private NavMeshAgent agent;

    [Header("효과")]
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
         좀비의 입장..
         플레이어한테 총알을 맞았을 때 
         총알을 맞으면 아야 소리 내줘야 하고
         HitEffect-> 방향 | 총알이 날라온 방향
         */
        if(!isDead)
        {
            HitEffect.transform.position = hitPosition;
            // hit 회전값을 바라보는 회전의 상태로 변환
            HitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);

            HitEffect.Play();

            Zombie_audio.PlayOneShot(HitClip);
        }

        base.OnDamage(Damage, hitPosition, hitNormal);
    }

    public override void Die()
    {

        //콜라이더 두개 있어서 하나 빼줘야함

        base.Die();

        Collider[] colls = GetComponents<Collider>();

        foreach(Collider c in colls)
        {
            c.enabled = false; // 비활성화
        }
        agent.isStopped = true;
        agent.enabled = false;
        Zombie_ani.SetTrigger("Die");
    }

    private void OnTriggerStay(Collider other)
    {
        // 닿고 있을 때 -> 지속적으로 호출
        /*
         Enter -> 닿기 시작
         Stay  -> 닿고 있을 때
         Exit  -> 닿는 것이 끝날 때
         */
        if(!isDead && Time.time >= LastAttackTimebet + TimebetAttack)
        {
            if(other.TryGetComponent(out LivingEntity e))
            {
                if(TargetEntity.Equals(e))
                {
                    LastAttackTimebet = Time.time;
                    // ClosestPoint -> 닿는 위치
                    // 즉 상대방 피격 위치와 피격 방향을 근사값으로 계산하는 메소드

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
            yield return null; // 한 프레임씩


        }
    }
}

