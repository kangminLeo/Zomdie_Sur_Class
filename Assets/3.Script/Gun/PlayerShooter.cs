using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    /*
     �� ��� ������ -> Gun
     Player input

     �� ������Ʈ �տ� ���߱� -> Animator
     
     
     */
    public Gun gun;

    // �ѱ� ��ġ ���߱� ���� Transform��

    public Transform GunPivot;
    public Transform LeftHand_mount;
    public Transform RightHand_mount;

    [SerializeField] private Animator animator;
    [SerializeField] private PlayerInput input;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        // Input ���õ� �̺�Ʈ ȣ��
        if(input.isFire)
        {
            gun.Fire();
        }
        else if(input.isReload)
        {
            if(gun.Reload())
            {
                animator.SetTrigger("Reload");
            }
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // ���� �������� ������ �Ȳ�ġ�� �̵�
        GunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        //IK �� ����Ͽ� �޼��� ��ġ�� ȸ���� �� ���� ������ ����
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand_mount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand_mount.rotation);

        //IK�� ����Ͽ� �������� ��ġ�� ȸ���� �� ������ �����̿� ����

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, RightHand_mount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, RightHand_mount.rotation);

    }
}
