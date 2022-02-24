using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDispenser : MonoBehaviour
{
    [SerializeField] GameObject healthPrefab;

    public void OnEnemyDestroyed()
    {
        if (ShouldRewardHealthPowerUp())
        {
            ShowHelthPowerUp();
        }
    }

    private bool ShouldRewardHealthPowerUp()
    {
        float random = Random.Range(0, 2);
        return random == 1;
    }

    private void ShowHelthPowerUp()
    {
        if (healthPrefab == null) return;

        Instantiate(healthPrefab,
                    transform.position,
                    Quaternion.identity);
    }
}
