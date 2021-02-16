using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager
{
    int score = 0;
    Text scoreText;
    public int Score 
    { 
        get
        {
            return score;
        }
        set
        {
            if (value > 0)
            {
                score = value;
                if(scoreText != null)
                    scoreText.text = $"Score : {Score}";
            }
        }
    }

    bool bGameOver { get; set; } = false;

    public void Init()
    {
        Score = 0;
        bGameOver = false;

        GameObject text = GameObject.Find("Score");
        if(text != null)
        {
            scoreText = text.GetComponent<Text>();
        }
    }
}
