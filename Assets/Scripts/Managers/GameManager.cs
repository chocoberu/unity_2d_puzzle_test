using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager
{
    int score = 0;
    int level = 1;

    Text scoreText;
    Text levelText;
    Image overUI;
    Button restartBtn;
    Button exitBtn;

    int[] levelArr = new int[4] { 5000, 15000, 30000, 50000 };

    // GameOver 시 델리게이트
    public Action gameOverPlayerAction;
    public Action gameOverBoardAction;

    // Restart 시 델리게이트
    public Action restartPlayerAction;
    public Action restartBoardAction;

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
            if (value >= 0)
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
        GameObject gameOverUI = GameObject.Find("GameOver");
        if(gameOverUI != null)
        {
            overUI = gameOverUI.GetComponent<Image>();
            restartBtn = GameObject.Find("RestartBtn").GetComponent<Button>();
            exitBtn = GameObject.Find("ExitBtn").GetComponent<Button>();

            // 버튼 이벤트 핸들러 설정
            restartBtn.onClick.AddListener(OnRestart);
            exitBtn.onClick.AddListener(OnExit);

            overUI.gameObject.SetActive(false);
        }
    }
    public void EndGame()
    {
        // TODO : GAME OVER UI 추가
        gameOverPlayerAction.Invoke();
        gameOverBoardAction.Invoke();
        if (overUI != null)
        {
            Debug.Log("OVER UI");
            overUI.gameObject.SetActive(true);
        }
    }
    void OnRestart()
    {
        overUI.gameObject.SetActive(false);

        restartBoardAction.Invoke();
        restartPlayerAction.Invoke();

        Score = 0;
        Level = 1;
    }
    void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL("https://www.google.com");
#else
        Application.Quit();
#endif
    }
}
