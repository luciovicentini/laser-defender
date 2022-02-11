using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTextUI;
    [SerializeField] float scoreTextTime = 1f;
    Canvas scoreCanvas;

    private void Awake()
    {
        scoreCanvas = GetScoreCanvas();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void show(int score)
    {
        if (scoreTextUI == null) return;
        
        scoreTextUI.text = score.ToString();
        TextMeshProUGUI instance = Instantiate(scoreTextUI, transform.position, Quaternion.identity, scoreCanvas.transform);
        Destroy(instance, scoreTextTime);
    }

    private Canvas GetScoreCanvas()
    {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.tag == "ScoreCanvas")
            {
                return canvas;
            }
        }

        return null;
    }
}
