using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    /*
     총 쏘기 재장전 -> Gun
     Player input

     건 오브젝트 손에 맞추기 -> Animator
     
     
     */
    public Gun gun;

    // 총기 위치 맞추기 위한 Transform들

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
        // Input 관련된 이벤트 호출
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
        // 총의 기준점을 오른쪽 팔꿈치로 이동
        GunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

        //IK 를 사용하여 왼손의 위치와 회전을 총 왼쪽 손잡이 맞춤
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand_mount.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand_mount.rotation);

        //IK를 사용하여 오른손의 위치와 회전을 총 오른쪽 손잡이에 맞춤

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        animator.SetIKPosition(AvatarIKGoal.RightHand, RightHand_mount.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, RightHand_mount.rotation);

    }
}
