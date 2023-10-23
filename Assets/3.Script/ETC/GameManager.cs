using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �̱��� _ ���� 2
    private static GameManager instance = null; // ĸ��ȭ
    public static GameManager Instance // �̱���
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>(); // �ν��Ͻ��� GameManager Ÿ���� ������Ʈ�� ã�´�.
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

    public int Score = 0; // ���ھ� 0
    public bool isGameOver { get; private set; } // ���ӿ��� �Ұ�
    private void Start()
    {
        FindObjectOfType<PlayerHealth>().onDead += EndGame; // �����Ҷ� PlayerHealth().onDead �޼ҵ带 EndGame �̺�Ʈ�� �߰�
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
