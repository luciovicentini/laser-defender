using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSceneCordinator : MonoBehaviour
{
    [SerializeField] float waitTillStart = 2f;
    [SerializeField] Transform playerPath;

    [SerializeField] float playerSpeed = 10;

    [SerializeField] Vector3 finalScale = new Vector3(2f, 2f, 1f);
    [SerializeField] GameObject lifeScorePanel;
    [SerializeField] float animatePanelScaleSpeed = 5f;
    [SerializeField] int startPanelScaleAnimation = 3;
    [SerializeField] TextMeshProUGUI countdownText;
    Vector3 originalScale;

    EnemySpawner enemySpawner;
    Player player;
    PlayerSelector playerSelector;

    int currentNodeIndex = 0;

    enum State
    {
        Idle,
        Animating,
        CountingDown,
        StartingGame,
        RunningGame,
    }

    State state = State.Idle;

    bool gameStarted = false;

    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        player = FindObjectOfType<Player>();
        playerSelector = FindObjectOfType<PlayerSelector>();
    }

    private void Update()
    {
        CheckState();
        switch (state)
        {
            case State.Idle:
            case State.RunningGame:
                break;
            case State.Animating:
                GetSpriteFromPlayerSelector();
                AnimatePlayer();
                AnimatePanel();
                break;
            case State.CountingDown:
                Countdown();
                break;
            case State.StartingGame:
                StartGame();
                break;
        }
    }

    private void CheckState()
    {
        if (playerSelector.Finished())
        {
            state = State.Animating;
        }

        if (PlayerAnimationFinished())
        {
            state = State.CountingDown;
        }

        if (CountdownFinished())
        {
            state = State.StartingGame;
        }

        if (gameStarted)
        {
            state = State.RunningGame;
        }
    }

    private void Countdown()
    {
        if (waitTillStart > 0)
        {
            UpdateCountdownText();
            waitTillStart -= Time.deltaTime;
        }
        else
        {
            waitTillStart = 0;
            HideCountdownText();
            StartGame();
        }
    }

    public void Start()
    {
        player.setShouldMove(false);
        player.transform.position = playerPath.GetChild(currentNodeIndex).position;
        currentNodeIndex++;
        originalScale = player.transform.localScale;
    }

    void StartGame()
    {
        gameStarted = true;
        player.setShouldMove(true);
        enemySpawner.StartSpawner();
    }

    void AnimatePlayer()
    {
        if (player == null) return;
        if (!PlayerAnimationFinished())
        {
            Vector3 targetPosition = playerPath.GetChild(currentNodeIndex).position;
            AnimatePosition(targetPosition);
            AnimateScale();
            if (player.transform.position == targetPosition)
            {
                currentNodeIndex++;
            }
        }
    }

    private void AnimatePosition(Vector3 targetPosition)
    {
        float delta = playerSpeed * Time.deltaTime;
        player.transform.position = Vector2.MoveTowards(player.transform.position, targetPosition, delta);
    }

    private void AnimateScale()
    {
        if (PlayerAnimationFinished()) return;

        if (currentNodeIndex == 2)
        {
            float delta = playerSpeed * Time.deltaTime;
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, finalScale, delta);
        }
        else
        {
            float delta = playerSpeed * Time.deltaTime;
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, originalScale, delta);
        }
    }

    private void AnimatePanel()
    {
        if (lifeScorePanel == null) return;

        if (currentNodeIndex < startPanelScaleAnimation) return;

        float scaleDelta = animatePanelScaleSpeed * Time.deltaTime;
        lifeScorePanel.transform.localScale = Vector3.Lerp(lifeScorePanel.transform.localScale, new Vector3(1f, 1f, 1f), scaleDelta);
    }

    public List<Transform> GetWaypoints()
    {
        List<Transform> waypoints = new List<Transform>();
        foreach (Transform child in playerPath)
        {
            waypoints.Add(child);
        }
        return waypoints;
    }

    private bool PlayerAnimationFinished()
    {
        return currentNodeIndex >= playerPath.childCount;
    }

    private void UpdateCountdownText()
    {
        int seconds = Mathf.FloorToInt(waitTillStart % 60);
        if (seconds >= 1)
        {
            UpdateCountdownText(seconds.ToString("0"));
        }
        else
        {
            UpdateCountdownText("Start");
        }
    }

    private void UpdateCountdownText(string text)
    {
        if (countdownText == null) return;
        countdownText.text = text;
    }

    private void HideCountdownText()
    {
        if (countdownText == null) return;
        countdownText.enabled = false;
    }

    private void GetSpriteFromPlayerSelector()
    {
        if (playerSelector == null) return;
        Sprite selectedShipSprite = playerSelector.GetSelectedSprite();
        if (selectedShipSprite == null) return;
        player.gameObject.GetComponent<SpriteRenderer>().sprite = selectedShipSprite;
    }

    private bool CountdownFinished() => waitTillStart == 0;
}