using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10;
    [SerializeField] float projectileLifetime = 2f;

    [SerializeField] [Range(0f, 1f)] float firingRate = 0.2f;

    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] [Range(0f, 1f)] float enemyFiringRate = 1f;
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumEnemyFiringRate = 0.5f;


    bool isFiring;

    Coroutine firingCoroutine;

    AudioPlayer audioPlayer;

    public bool IsFiring() => isFiring;
    public void SetIsFiring(bool value) => isFiring = value;

    private void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Start()
    {
        if (useAI)
        {
            isFiring = true;
            if (projectileSpeed > 0)
            {
                projectileSpeed *= -1;
            }
        }
    }

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

            float firingRate = useAI ? GetEnemyRandomFireRate() : this.firingRate;
            Logging(firingRate);
            PlayShootingSFX();
            yield return new WaitForSeconds(firingRate);
        }
    }

    private void Logging(float firingRate)
    {
        Debug.Log("Firing Rate = " + firingRate);
        string whoIsFiring = useAI ? "AI" : "Player";
        Debug.Log(whoIsFiring + " is firing");
    }

    void PlayShootingSFX()
    {
        if (audioPlayer == null) return;

        audioPlayer.PlayShootingClip();
    }

    float GetEnemyRandomFireRate()
    {
        float fireRate = Random.Range(enemyFiringRate - firingRateVariance,
                                        enemyFiringRate + firingRateVariance);

        return Mathf.Clamp(fireRate, minimumEnemyFiringRate, float.MaxValue);
    }
}
