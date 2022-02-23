using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    Player player;
    PlayerSelector playerSelector;
    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player>();
        playerSelector = FindObjectOfType<PlayerSelector>();
    }

    void OnMove(InputValue value)
    {
        PlayerSelectorOnMove(value);
        PlayerOnMove(value);
    }

    void OnFire(InputValue value)
    {
        PlayerSelectorOnFire();
        PlayerOnFire(value);
    }

    private void PlayerSelectorOnMove(InputValue value)
    {
        if (playerSelector == null) return;
        playerSelector.OnMove(value);
    }

    private void PlayerSelectorOnFire()
    {
        if (playerSelector == null) return;
        playerSelector.OnFire();
    }
    private void PlayerOnMove(InputValue value)
    {
        if (player == null) return;
        player.OnMove(value);
    }

    private void PlayerOnFire(InputValue value)
    {
        if (player == null) return;
        player.OnFire(value);
    }
}
