using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    private const string SHIP_SELECT_TAG = "ShipSelect";
    [SerializeField] float selectionMoveSpeed = 1f;
    [SerializeField] TextMeshProUGUI shipNameTextMesh;
    [SerializeField] Button nextShipButton;
    [SerializeField] Button prevShipButton;

    [SerializeField] GameObject shipSelectionCanvas;
    List<Transform> playerShips = new List<Transform>();
    List<float> shipXPositions = new List<float>();

    int selectedShipIndex = 0;
    enum State
    {
        Idle,
        Left,
        Right,
        Selected,
        Finished,
    }
    State state = State.Idle;

    bool readyToMove = true;
    bool readyToSelect = true;

    Sprite selectedSprite;

    void Awake()
    {
        GetPlayerSelectionShips();
    }

    void GetPlayerSelectionShips()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.tag == SHIP_SELECT_TAG)
            {
                playerShips.Add(child);
                shipXPositions.Add(child.position.x);
            }
        }
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                readyToMove = true;
                readyToSelect = true;
                ToggleButtons(true);
                ToggleShipName(true);
                HideFirstAndLastShipButtons();
                return;
            case State.Right:
                readyToSelect = false;
                ToggleButtons(false);
                HandleShipNameChange();
                AnimateShipSelection();
                return;
            case State.Left:
                readyToSelect = false;
                ToggleButtons(false);
                HandleShipNameChange();
                AnimateShipSelection();
                return;
            case State.Selected:
                readyToMove = false;
                readyToSelect = false;
                SaveSelectedSprite();
                DestroyUI();
                HideShipsExceptSelected();
                AnimateSelectedShipOffScreen();
                return;
        }
    }

    public void OnMove(InputValue value)
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

    public void OnFire()
    {
        if (!readyToSelect) return;
        state = State.Selected;
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

    private void SaveSelectedSprite()
    {
        selectedSprite = playerShips[selectedShipIndex].gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    private void AnimateSelectedShipOffScreen()
    {
        float delta = selectionMoveSpeed * Time.deltaTime;
        Vector2 target = new Vector2(transform.position.x, 20);
        transform.position = Vector2.MoveTowards(transform.position, target, delta);

        if (transform.position.y == target.y)
        {
            RemoveShipsFromScene();
            state = State.Finished;
        }
    }

    private void RemoveShipsFromScene()
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.tag == SHIP_SELECT_TAG)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void SelectNextShip()
    {
        if (selectedShipIndex >= playerShips.Count - 1) return;
        readyToMove = false;
        selectedShipIndex++;
        state = State.Right;
    }

    void SelectPrevShip()
    {
        if (selectedShipIndex <= 0) return;
        readyToMove = false;
        selectedShipIndex--;
        state = State.Left;
    }

    void AnimateShipSelection()
    {
        float v = shipXPositions[selectedShipIndex];
        float delta = selectionMoveSpeed * Time.deltaTime;
        Vector2 target = new Vector2(v * -1, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, delta);

        if (Mathf.Abs(transform.position.x) == Mathf.Abs(v))
        {
            state = State.Idle;
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

    public bool Finished() => state == State.Finished;

    public Sprite GetSelectedSprite() => selectedSprite;
}