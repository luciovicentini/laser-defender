using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDisplay : MonoBehaviour
{
    Slider healthSlider;
    TextMeshProUGUI scoreText;

    Health playerHealth;

    ScoreKeeper scoreKeeper;

    private void Awake()
    {
        playerHealth = FindObjectOfType<Player>().GetComponent<Health>();
        healthSlider = GetComponentInChildren<Slider>();
        scoreText = GetComponentInChildren<TextMeshProUGUI>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    private void Start()
    {
        healthSlider.maxValue = playerHealth.GetHealth();
    }

    private void Update()
    {
        SetHealthSliderValue(playerHealth.GetHealth());
        SetScoreText(scoreKeeper.GetCurrentScore());
    }


    void SetHealthSliderValue(float health) => healthSlider.value = health;

    void SetScoreText(int score) => scoreText.text = Utils.NormalizeScore(score);

}