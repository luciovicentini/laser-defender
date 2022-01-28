using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10;
    [SerializeField] float projectileLifetime = 2f;

    [SerializeField] float firingRate = 0.2f;

    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float enemyFiringRate = 1f;
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumEnemyFiringRate = 0.5f;


    bool isFiring;

    Coroutine firingCoroutine;

    public bool IsFiring() => isFiring;
    public void SetIsFiring(bool value) => isFiring = value;

    // Start is called before the first frame update
    void Start()
    {
        if (useAI)
        {
            isFiring = true;
            if (projectileSpeed > 0) {
                projectileSpeed *= -1;
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject projectileShooted = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectileShooted.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.velocity = transform.up * projectileSpeed;
            }
            Destroy(projectileShooted, projectileLifetime);
            
            float firingRate = useAI ? GetRandomFireRate() : this.firingRate;
            yield return new WaitForSeconds(firingRate);
        }
    }
    
    float GetRandomFireRate()
    {
        float fireRate = Random.Range(enemyFiringRate - firingRateVariance,
                                        enemyFiringRate + firingRateVariance);

        return Mathf.Clamp(fireRate, minimumEnemyFiringRate, float.MaxValue);
    }
}
