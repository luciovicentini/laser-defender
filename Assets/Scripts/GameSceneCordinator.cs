using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCordinator : MonoBehaviour
{
    [SerializeField] float waitTillStart = 2f;
    [SerializeField] Transform playerPath;

    [SerializeField] float playerSpeed = 10;

    [SerializeField] Vector3 finalScale = new Vector3(2f, 2f, 1f);
    Vector3 originalScale;

    EnemySpawner enemySpawner;
    Player player;

    bool startAnimation = false;

    int currentNodeIndex = 0;

    private void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        player = FindObjectOfType<Player>();
    }

    public void StartGame()
    {
        player.setShouldMove(false);
        player.transform.position = playerPath.GetChild(currentNodeIndex).position;
        currentNodeIndex++;
        originalScale = player.transform.localScale;
        startAnimation = true;
        StartCoroutine(StartScene());
    }

    private void Update()
    {
        if (startAnimation) AnimatePlayer();
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(waitTillStart);
        startAnimation = false;
        player.setShouldMove(true);
        enemySpawner.StartSpawner();
    }

    void AnimatePlayer()
    {
        if (player == null) return;
        if (currentNodeIndex < playerPath.childCount)
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
        if (currentNodeIndex == 2)
        {
            player.transform.localScale = Vector3.Lerp(player.transform.localScale, finalScale, Time.deltaTime);
        }
        else
        {
            player.transform.localScale = Vector3.Lerp(originalScale, player.transform.localScale, Time.deltaTime);
        }
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

    public void SetSelectedSprite(Sprite playerSprite)
    {
        player.gameObject.GetComponent<SpriteRenderer>().sprite = playerSprite;
    }
}
