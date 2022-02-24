using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;

    [SerializeField] float bottomMarginDestroy = 2f;
    [SerializeField] int healthAmount = 10;

    void Update()
    {
        AnimateScrollingMovement();
        DestroyOffScreen();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    private void AnimateScrollingMovement()
    {
        gameObject.transform.position -= new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }

    private void DestroyOffScreen()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y < Mathf.Abs(bottomMarginDestroy))
        {
            Destroy(this.gameObject);
        }
    }

    internal int GetHealthAmount() => healthAmount;
}
