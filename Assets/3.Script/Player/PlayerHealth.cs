using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : LivingEntity // Monobehavior 상속받고있음
{
    public Slider healthSlider;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemDropClip;

    private AudioSource playerAudio;
    private Animator player_ani;

    private PlayerMovement player_move;
    private PlayerShooter player_shooter;

    private void Awake()
    {
        TryGetComponent(out playerAudio);
        TryGetComponent(out player_ani);
        TryGetComponent(out player_move);
        TryGetComponent(out player_shooter);
    }

    protected override void OnEnable()
    {
        base.OnEnable(); // ->부모로부터 클래스 메소드 호출
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = StartHealth;
        healthSlider.value = CurrentHealth;

        // 죽었을 때 Movement Shooter를 비활성화 할꺼기 때문에
        // 여기서는 확인 차 활성화 해야됨

        player_move.enabled = true;
        player_shooter.enabled = true;
    }

    public override void OnDamage(float Damage, Vector3 hitPosition, Vector3 hitNormal)
    {
        if(!isDead)
        {
            playerAudio.PlayOneShot(hitClip);
        }

        base.OnDamage(Damage, hitPosition, hitNormal);

        healthSlider.value = CurrentHealth; // health update
    }

    public override void Die()
    {
        base.Die();

        healthSlider.gameObject.SetActive(false);
        // 시각적 효과 청각적 효과
        player_ani.SetTrigger("Die");
        playerAudio.PlayOneShot(deathClip);

        // 죽었을 대 movement shooter를 비활성화
        player_move.enabled = false;
        player_shooter.enabled = false;
    }
}
