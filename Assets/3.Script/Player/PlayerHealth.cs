using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : LivingEntity // Monobehavior ��ӹް�����
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
        base.OnEnable(); // ->�θ�κ��� Ŭ���� �޼ҵ� ȣ��
        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = StartHealth;
        healthSlider.value = CurrentHealth;

        // �׾��� �� Movement Shooter�� ��Ȱ��ȭ �Ҳ��� ������
        // ���⼭�� Ȯ�� �� Ȱ��ȭ �ؾߵ�

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
        // �ð��� ȿ�� û���� ȿ��
        player_ani.SetTrigger("Die");
        playerAudio.PlayOneShot(deathClip);

        // �׾��� �� movement shooter�� ��Ȱ��ȭ
        player_move.enabled = false;
        player_shooter.enabled = false;
    }
}
