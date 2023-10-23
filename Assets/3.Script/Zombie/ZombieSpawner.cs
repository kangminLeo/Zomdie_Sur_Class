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
        //SpawnPoint ����
        SetupSpawnPoint();
    }

    private void SetupSpawnPoint()
    {
        spawnPoint = new Transform[transform.childCount];

        for(int i = 0; i < spawnPoint.Length; i++)
        {
            //GetChild(intdex) �ڽ� ��ü�� ������� ������ �� �� ����Ѵ�.
            spawnPoint[i] = transform.GetChild(i).transform;
        }
    }

    private void Update()
    {

        
        // ���� ������ ���
        if (GameManager.Instance != null && GameManager.Instance.isGameOver == true)
        {
            return;
        }

        if(zombieList.Count <= 0)
        {
            SpawnWave();
            // ���̺� �ø��� �޼ҵ� �־��ּ���
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
        // ���̺� ����
        Wave++;
        // ���� ���� �� ���� ������� ������.
        int count = Mathf.RoundToInt(Wave * 2f);

        for(int i = 0; i < count; i++)
        {
            CreateZombie();
        }
    }

    private void CreateZombie()
    {
        /*
         Zombie data �����ϰ� ������.
         Zombie SpawnPoint �����ϰ� ������.

        ���� ���� �Ǿ��� �� -> Event �߰�
        1. List ���� ����
        2. ���� ������Ʈ ����
        3. ���� ���
         
         */

        ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];
        Transform point = spawnPoint[Random.Range(0, spawnPoint.Length)];

        ZombieController zombie = Instantiate(this.zombie, point.position, point.rotation);
        zombie.Setup(data);
        zombieList.Add(zombie);
        
        // �������� �¾

        // �͸��Լ� ���
        zombie.onDead += () => { zombieList.Remove(zombie); };
        zombie.onDead += () => { Destroy(zombie.gameObject, 10f); };
        zombie.onDead += () => { GameManager.Instance.AddScore(10); };
    }

}
