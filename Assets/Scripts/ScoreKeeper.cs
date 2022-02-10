using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    ScoreKeeper instance;

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Awake() {
        ManageSingleton();
    }

    int currentScore = 0;

    public int GetCurrentScore() => currentScore;

    public void AddToCurrentScore(int value)
    {
        currentScore += value;
        Mathf.Clamp(currentScore, 0, int.MaxValue);
    }

    public void ResetCurrentScore() => currentScore = 0;

    public string NormalizedScore()
    {
        return currentScore.ToString("000000000");
    }
}
