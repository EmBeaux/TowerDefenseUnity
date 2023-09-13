using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour
{
    public static InfoManager instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI coinsText;

    public int score = 0;
    public int level = 1;
    public int coins = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one InfoManager in scene!");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        scoreText.text = "Score: " + score;
        levelText.text = "Level: " + level;
        coinsText.text = "Coins: " + coins;
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = "Score: " + this.score;
    }

    public void SetLevel(int level)
    {
        this.level = level;
        levelText.text = "Level: " + this.level;
    }

    public void AddCoins(int coins)
    {
        this.coins += coins;
        coinsText.text = "Coins: " + this.coins;
    }
}
