using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
[Header("Score")]
public int kills;
public int enemyKills;
public Text playerKillCounter;
public Text enemykillCounter;
public Text Maintext;

    public GameOver gameManager;
    private bool isWin;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("kills"))
        {
            kills = PlayerPrefs.GetInt("0");
        }
        else if(PlayerPrefs.HasKey("enemyKills"))
        {
            enemyKills = PlayerPrefs.GetInt("0");
        }
    }
    private void Update()

    {
        StartCoroutine(WinOrLose());

    }
        

    IEnumerator WinOrLose()
            {
                playerKillCounter.text = "" + kills;
                enemykillCounter.text = "" + enemyKills;

                if (kills >= 10 && !isWin)
                {
            gameManager.gameOver();
                    Maintext.text = "Blue Team Victory";
                    PlayerPrefs.SetInt("kills", kills);
                    Time.timeScale = 0f;
                    yield return new WaitForSeconds(5f);
                    SceneManager.LoadScene("TDMArena");
                }





                else if(enemyKills >= 10 && !isWin)
                {
            isWin = true;
            gameManager.gameOver();
            Maintext.text = "Red Team Victory";
                    PlayerPrefs.SetInt("enemyKills", enemyKills);
                    Time.timeScale = 0f;
                    yield return new WaitForSeconds(5f);
                    SceneManager.LoadScene("TDMArena");


                }
            }
}
