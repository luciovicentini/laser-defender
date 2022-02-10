using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Shooter
{

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] [Range(0f, 2f)] float enemyFiringRate = 1f;
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumEnemyFiringRate = 0.5f;

    float firingRate;
    // Start is called before the first frame update
    void Start()
    {
        base.isFiring = true;
        if (projectileSpeed > 0)
        {
            projectileSpeed *= -1;
        }
        firingRate = GetEnemyRandomFireRate();
    }

    // Update is called once per frame
    void Update()
    {
        Fire(projectilePrefab, firingRate);
    }

    float GetEnemyRandomFireRate()
    {
        float fireRate = Random.Range(enemyFiringRate - firingRateVariance,
                                        enemyFiringRate + firingRateVariance);

        return Mathf.Clamp(fireRate, minimumEnemyFiringRate, float.MaxValue);
    }
}
