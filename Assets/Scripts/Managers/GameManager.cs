using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager
{
    int score = 0;
    int level = 1;

    Text scoreText;
    Text levelText;

    int[] levelArr = new int[4] { 5000, 15000, 30000, 50000 };

    public void AddScore()
    {
        Score += 500;
    }

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
                if (level < 4 && score >= levelArr[level - 1])
                    Level++;
            }
        }
    }
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            if (levelText != null)
                levelText.text = $"Lv {level}";
        }
    }

    bool bGameOver { get; set; } = false;

    public void Init()
    {
        Score = 0;
        bGameOver = false;

        GameObject stext = GameObject.Find("Score");
        if(stext != null)
        {
            scoreText = stext.GetComponent<Text>();
        }
        GameObject ltext = GameObject.Find("Level");
        if(ltext != null)
        {
            levelText = ltext.GetComponent<Text>();
        }
    }
    public void EndGame()
    {
        // TODO : GAME OVER UI 추가
    }
}
