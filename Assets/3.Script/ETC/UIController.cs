using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*
     ź�� ǥ�ÿ� �ؽ�Ʈ
    ���� ǥ�� �ؽ�Ʈ -> GameManager -> ����
    �� ���̺�
    ���� ���� ������Ʈ
     */
    [SerializeField] private Text AmmoText;
    [SerializeField] private Text ScoreText;
    [SerializeField] private Text Wave_Text;

    [SerializeField] private GameObject Gameover_ob;


    // ź�� ������Ʈ
    public void Update_AmmoText(int magAmmo, int Remain)
    {
        // 25 / 100

        AmmoText.text = string.Format("{0} / {1}", magAmmo, Remain);
    }

    public void Update_ScoreText(int newScore)
    {
        // Score : 00
        ScoreText.text = string.Format("Score : {0}", newScore);
    }

    public void Update_WaveText(int Wave, int Count)
    {
        // Wave : 0
        // Zombie
        // Wave_Text.text = string.Format("Wave : {0}\nZombie Left : {1}", Wave, Count);
    }
    public void SetActive_Gameover(bool isAct)
    {
        Gameover_ob.SetActive(isAct);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
