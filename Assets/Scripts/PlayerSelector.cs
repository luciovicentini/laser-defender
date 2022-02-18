using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField] float selectionMoveSpeed = 1f;
    [SerializeField] TextMeshProUGUI shipNameTextMesh;
    [SerializeField] Button nextShipButton;
    [SerializeField] Button prevShipButton;

    [SerializeField] GameObject shipSelectionCanvas;
    List<Transform> playerShips = new List<Transform>();
    List<float> shipXPositions = new List<float>();

    int selectedShipIndex = 0;
    enum SelectionState
    {
        Idle,
        Left,
        Right,
        Selected,
    }
    SelectionState moveState = SelectionState.Idle;

    bool readyToMove = true;

    GameSceneCordinator gameSceneCordinator;

    void Awake()
    {
        GetPlayerSelectionShips();
        gameSceneCordinator = FindObjectOfType<GameSceneCordinator>();
    }

    void GetPlayerSelectionShips()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.tag == "ShipSelect")
            {
                playerShips.Add(child);
                shipXPositions.Add(child.position.x);
            }
        }
    }

    void Update()
    {
        if (moveState == SelectionState.Idle)
        {
            ToggleButtons(true);
            ToggleShipName(true);
            HideFirstAndLastShipButtons();
            return;
        }
        if (moveState == SelectionState.Right)
        {
            ToggleButtons(false);
            HandleShipNameChange();
            AnimateShipSelection();

        }
        if (moveState == SelectionState.Left)
        {
            ToggleButtons(false);
            HandleShipNameChange();
            AnimateShipSelection();
        }
        if (moveState == SelectionState.Selected)
        {
            DestroyUI();
            HideShipsExceptSelected();
            gameSceneCordinator.SetSelectedSprite(GetSelectedSprite());
            AnimateSelectedShipOffScreen();
        }
    }

    private void DestroyUI()
    {
        Destroy(shipSelectionCanvas.gameObject);
    }

    private void HideShipsExceptSelected()
    {
        for (int i = 0; i < playerShips.Count; i++)
        {
            if (i == selectedShipIndex) continue;
            playerShips[i].gameObject.SetActive(false);
        }
    }

    private Sprite GetSelectedSprite()
    {
        Sprite sprite = playerShips[selectedShipIndex].gameObject.GetComponent<SpriteRenderer>().sprite;
        Debug.Log(sprite);
        return sprite;
    }

    private void AnimateSelectedShipOffScreen()
    {
        float delta = selectionMoveSpeed * Time.deltaTime;
        Vector2 target = new Vector2(transform.position.x, 20);
        transform.position = Vector2.MoveTowards(transform.position, target, delta);

        if (transform.position.y == target.y)
        {
            Destroy(this.gameObject);
            gameSceneCordinator.StartGame();
        }
    }

    void OnMove(InputValue value)
    {
        if (!readyToMove) return;
        Vector2 rawInput = value.Get<Vector2>();

        if (rawInput.x > 0)
        {
            SelectNextShip();
        }
        if (rawInput.x < 0)
        {
            SelectPrevShip();
        }
    }

    void SelectNextShip()
    {
        if (selectedShipIndex >= playerShips.Count - 1) return;
        readyToMove = false;
        selectedShipIndex++;
        moveState = SelectionState.Right;
    }

    void SelectPrevShip()
    {
        if (selectedShipIndex <= 0) return;
        readyToMove = false;
        selectedShipIndex--;
        moveState = SelectionState.Left;
    }

    /* void AnimateToNextShip()
    {
        float v = shipXPositions[selectedShipIndex];
        float delta = selectionMoveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(v * -1, transform.position.y), delta);

        if (Mathf.Abs(transform.position.x) == Mathf.Abs(v))
        {
            readyToMove = true;
            moveState = MoveState.Idle;
        }
    } */

    void AnimateShipSelection()
    {
        float v = shipXPositions[selectedShipIndex];
        float delta = selectionMoveSpeed * Time.deltaTime;
        Vector2 target = new Vector2(v * -1, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, delta);

        if (Mathf.Abs(transform.position.x) == Mathf.Abs(v))
        {
            readyToMove = true;
            moveState = SelectionState.Idle;
        }
    }

    void HideFirstAndLastShipButtons()
    {
        if (prevShipButton == null) return;
        if (nextShipButton == null) return;

        bool showPrevButton = true;
        if (selectedShipIndex == 0)
        {
            showPrevButton = false;
        }
        prevShipButton.gameObject.SetActive(showPrevButton);

        bool showNextButton = true;
        if (selectedShipIndex == playerShips.Count - 1)
        {
            showNextButton = false;
        }
        nextShipButton.gameObject.SetActive(showNextButton);
    }

    void ToggleButtons(bool shouldShown)
    {
        if (prevShipButton == null || nextShipButton == null) return;

        prevShipButton.gameObject.SetActive(shouldShown);
        nextShipButton.gameObject.SetActive(shouldShown);
    }

    void ToggleShipName(bool shouldShown)
    {
        if (shipNameTextMesh == null) return;
        shipNameTextMesh.gameObject.SetActive(shouldShown);
    }

    void SetShipName(string name)
    {
        shipNameTextMesh.text = name;
    }

    string GetShipNameFromGameObject(GameObject shipGameObject)
    {
        string shipFullName = shipGameObject.name;
        return shipFullName.Replace("Player ", "");
    }

    void HandleShipNameChange()
    {
        ToggleShipName(false);
        string shipName = GetShipNameFromGameObject(playerShips[selectedShipIndex].gameObject);
        SetShipName(shipName);
    }

    void OnFire()
    {
        moveState = SelectionState.Selected;
    }
}
