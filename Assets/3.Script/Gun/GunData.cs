using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GunData", fileName = "Gun_Data")]
public class GunData : ScriptableObject
{
    /*
        공격력 => float
        연사력 => float => 코루틴 써서 막기
        재장전 시간 => float 
        처음 주어질 전체 총알량 => int
        총소리 => Audio Clip
        재장전 소리 => Audio Clip
        탄창용량 => int
     
     */

    public float Damage = 25f;
    public float TimebetFire = 0.12f;
    public float ReloadTime = 1.8f;
    public int MagCapacity = 30;
    public int StartAmmoRemain = 100;

    public AudioClip Shot_clip;
    public AudioClip Reload_clip;
    
}
