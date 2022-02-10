using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    // [Header("General")]
    [SerializeField] protected float projectileSpeed = 10;
    [SerializeField] protected float projectileLifetime = 2f;
    // [SerializeField] [Range(0f, 1f)] protected float firingRate = 0.2f;

    protected bool isFiring;

    Coroutine firingCoroutine;

    AudioPlayer audioPlayer;

    public bool IsFiring() => isFiring;
    public void SetIsFiring(bool value) => isFiring = value;

    private void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    protected void Fire(GameObject gameObject, float firingRate)
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously(gameObject, firingRate));
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously(GameObject gameObject, float firingRate)
    {
        while (true)
        {
            GameObject projectileShooted = Instantiate(gameObject, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectileShooted.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.velocity = transform.up * projectileSpeed;
            }
            Destroy(projectileShooted, projectileLifetime);

            // float firingRate = useAI ? GetEnemyRandomFireRate() : this.firingRate;
            PlayShootingSFX();
            yield return new WaitForSeconds(firingRate);
        }
    }

    void PlayShootingSFX()
    {
        if (audioPlayer == null) return;

        audioPlayer.PlayShootingClip();
    }
}
