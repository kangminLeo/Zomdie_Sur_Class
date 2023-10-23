using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public ZombieData[] zombieDatas;
    public ZombieController zombie;


    [SerializeField] private Transform[] spawnPoint;

    private List<ZombieController> zombieList = new List<ZombieController>();

    private int Wave;

    private void Awake()
    {
        //SpawnPoint 설정
        SetupSpawnPoint();
    }

    private void SetupSpawnPoint()
    {
        spawnPoint = new Transform[transform.childCount];

        for(int i = 0; i < spawnPoint.Length; i++)
        {
            //GetChild(intdex) 자식 객체를 순서대로 가지고 올 때 사용한다.
            spawnPoint[i] = transform.GetChild(i).transform;
        }
    }

    private void Update()
    {

        
        // 게임 오버일 경우
        if (GameManager.Instance != null && GameManager.Instance.isGameOver == true)
        {
            return;
        }

        if(zombieList.Count <= 0)
        {
            SpawnWave();
            // 웨이브 늘리는 메소드 넣어주세요
        }
        // UI update
        
        UpdateUI();
    }
    private void UpdateUI()
    {
        UIController.instance.Update_WaveText(Wave, zombieList.Count);
        Debug.Log(zombieList.Count);
        //Debug.Log(Wave);
    }

    private void SpawnWave()
    {
        // 웨이브 증가
        Wave++;
        // 좀비 생성 및 좀비 몇마리인지 결정함.
        int count = Mathf.RoundToInt(Wave * 2f);

        for(int i = 0; i < count; i++)
        {
            CreateZombie();
        }
    }

    private void CreateZombie()
    {
        /*
         Zombie data 랜덤하게 정해줌.
         Zombie SpawnPoint 랜덤하게 정해줌.

        좀비가 다이 되었을 때 -> Event 추가
        1. List 에서 삭제
        2. 좀비 오브젝트 삭제
        3. 점수 계산
         
         */

        ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];
        Transform point = spawnPoint[Random.Range(0, spawnPoint.Length)];

        ZombieController zombie = Instantiate(this.zombie, point.position, point.rotation);
        zombie.Setup(data);
        zombieList.Add(zombie);
        
        // 새생명이 태어남

        // 익명함수 사용
        zombie.onDead += () => { zombieList.Remove(zombie); };
        zombie.onDead += () => { Destroy(zombie.gameObject, 10f); };
        zombie.onDead += () => { GameManager.Instance.AddScore(10); };
    }

}
