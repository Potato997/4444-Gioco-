using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public int score = 0;
    public Text record;
    public Text losetext;
    GameObject lose;

    void Start()
    {
        lose = GameObject.Find("Endgame");
        lose.SetActive(false);
    }
    public void Aumentapunteggio()
    {
        score += 1;
        record.text = "SCORE: " + score;
    }

    void Update()
    {
        if(!GameObject.FindGameObjectWithTag("Base"))
        StartCoroutine(Endedgame());
    }

    IEnumerator Endedgame()
    {
        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
            losetext.text = "NEW HIGHSCORE!" + "\n" + score + "!";
        }
        else
            //GameObject.Find("LoseText").GetComponent<Animation>().Play();
        losetext.text = "You lose!" + "\n" + "\n" + "Your score: " +score;
        yield return new WaitForSeconds(3.5f);
        Destroy(losetext);
        lose.SetActive(true);
    }
}