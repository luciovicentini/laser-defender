using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    private void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    private void Start()
    {
        string text = Utils.NormalizeScore(0);
        if (scoreKeeper != null)
        {
            text = Utils.NormalizeScore(scoreKeeper.GetCurrentScore());
        }
        scoreText.text = text;
    }
}
