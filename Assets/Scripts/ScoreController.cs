using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI highScoreUI;
    private int score = 0;
    private int highScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HIGHSCORE");
    }

    // Update is called once per frame
    void Update()
    {
        scoreUI.text = score.ToString();
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HIGHSCORE", highScore);
            PlayerPrefs.Save();
        }
        highScoreUI.text = highScore.ToString();
    }

    public void addScore(int plusScore)
    {
        score += plusScore;
    }
}
