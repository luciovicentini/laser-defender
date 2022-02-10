using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : Shooter
{

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float firingRate = 1f;

    // Update is called once per frame
    void Update()
    {
        Fire(projectilePrefab, firingRate);
    }
}
