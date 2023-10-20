using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    /*
    �Ѿ� -> LineRender -> RayCast 
    �Ѿ� ��Ÿ�
    �߻�� ��ġ
    GunData
    Effect
    ���� ���� -< Enum
                ������
                źâ�� ����� ��
                �߻� �غ�
     audio Source

    Method
    �߻� -> Fire
    ������ -> Reload
    Effect Play
    



     */
    public enum State
    {
        Ready, // �߻� �غ�
        Empty, // �Ѿ� 0
        Reloading // ������
    }

    public State state { get; private set; }
    // �Ѿ��� �߻�� ��ġ
    public Transform fire_Transform;

    // �Ѿ� Line Renderer
    public LineRenderer lineRenderer;

    //�Ѿ� �߻� source
    private AudioSource audioSource;

    //��Ÿ�
    private float Distance = 50f;

    //�� Data
    public GunData data;

    public ParticleSystem shot_Effect;
    public ParticleSystem shell_Effect;

    private float LastFireTime;

    public int ammoRemain = 100;
    public int magAmmo;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        
        lineRenderer.positionCount = 2;
        // ������Ʈ ��Ȱ��ȭ
        lineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        ammoRemain = data.StartAmmoRemain;
        magAmmo = data.MagCapacity;

        state = State.Ready;

        LastFireTime = 0;
    }

    public void Fire()
    {
        // �÷��̾��� ���� �� ���°� �غ�����̸鼭
        // ������ �߻� �ð��� ���� �ð����� ���� �� �߻� ����

        if(state.Equals(State.Ready) && Time.time >= LastFireTime + data.TimebetFire)
        {
            LastFireTime = Time.time;
            //�߻�
            Shot();
        }
    }

    public void Shot()
    {
        // �� -> Raycast
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;
        
        if(Physics.Raycast(fire_Transform.position, fire_Transform.forward, out hit, Distance))
        {
            // �Ѿ��� �¾��� ���
            // �츮�� ���� �������̽��� ������ �ͼ�
            // ���� ������Ʈ���� �������� �����.
            IDamage target = hit.collider.GetComponent<IDamage>();

            if(target != null)
            {
                target.OnDamage(data.Damage, hit.point, hit.normal);
            }
            
            //if(hit.collider.TryGetComponent(out IDamage damage))
            //{
            //    target.OnDamage(data.Damage, hit.point, hit.normal);
            //}

            hitPosition = hit.point;
        }
        else
        {
            // Ray�� �ٸ� ��ü�� �浹���� �ʾ��� ���
            // ź���� �ִ� �����Ÿ����� ��������
            hitPosition = fire_Transform.position + fire_Transform.forward * Distance;
        }
        // ���� �� ����Ʈ �÷���
        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;
        if(magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        shot_Effect.Play();
        shell_Effect.Play();
        // �Ҹ� 
        audioSource.PlayOneShot(data.Shot_clip);

        // ���� ������ ����
        lineRenderer.SetPosition(0, fire_Transform.position);
        lineRenderer.SetPosition(1, hitPosition);

        //���� �׸� �׸�
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        lineRenderer.enabled = false;

    }

    public bool Reload()
    {
        // ���� �������� �ʿ����� ������ Return�� �޼ҵ�

        // �̹� ���������̰ų�, �Ѿ��� ���ų�, źâ�� �̹� �Ѿ��� ������ ���(30��)
                    // => false


        if(state.Equals(State.Reloading) || ammoRemain <= 0 || magAmmo >= data.MagCapacity)
        {
            return false;
        }

        // ���� �� �� �ִ� ����
        StartCoroutine(Reload_co());
        return true;
    }

    private IEnumerator Reload_co()
    {
        state = State.Reloading;
        audioSource.PlayOneShot(data.Reload_clip);
        yield return new WaitForSeconds(data.ReloadTime);

        // ������ �Ŀ� ���
        int ammofill = data.MagCapacity - magAmmo;

        // źâ�� ä���� �� ź���� ���� ź�ຸ�� ���ٸ�
        // ä������ ź����� ���� ź�� ���� ���� ���δ�.
        if(ammoRemain < ammofill)
        {
            ammofill = ammoRemain;
        }
        // źâ�� ä��� ��ü źâ�� ���� ���δ�.
        magAmmo += ammofill;
        ammoRemain -= ammofill;
        state = State.Ready;
    }
}
