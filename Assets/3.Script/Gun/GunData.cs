using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GunData", fileName = "Gun_Data")]
public class GunData : ScriptableObject
{
    /*
        ���ݷ� => float
        ����� => float => �ڷ�ƾ �Ἥ ����
        ������ �ð� => float 
        ó�� �־��� ��ü �Ѿ˷� => int
        �ѼҸ� => Audio Clip
        ������ �Ҹ� => Audio Clip
        źâ�뷮 => int
     
     */

    public float Damage = 25f;
    public float TimebetFire = 0.12f;
    public float ReloadTime = 1.8f;
    public int MagCapacity = 30;
    public int StartAmmoRemain = 100;

    public AudioClip Shot_clip;
    public AudioClip Reload_clip;
    
}
