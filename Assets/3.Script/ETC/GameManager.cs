using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 _ 형태 2
    private static GameManager instance = null; // 캡슐화
    public static GameManager Instance // 싱글톤
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>(); // 인스턴스는 GameManager 타입의 오브젝트를 찾는다.
            }
            return instance;
        }
    }
    private void Awake()
    {
        if(Instance == null)
        {
            Destroy(gameObject);
        }
    }

    public int Score = 0; // 스코어 0
    public bool isGameOver { get; private set; } // 게임오버 불값
    private void Start()
    {
        FindObjectOfType<PlayerHealth>().onDead += EndGame; // 시작할때 PlayerHealth().onDead 메소드를 EndGame 이벤트에 추가
    }

    public void EndGame()
    {
        isGameOver = true;
        UIController.instance.SetActive_Gameover(true);
    }

    public void AddScore(int newScore)
    {
        if(!isGameOver)
        {
            Score += newScore;
            UIController.instance.Update_ScoreText(Score);
        }
    }
}
