using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] int health = 50;
    [SerializeField] int score = 50;
    [SerializeField] ParticleSystem hitEffect;

    [SerializeField] bool applyCameraShake;


    CameraShake cameraShake;

    AudioPlayer audioPlayer;
    ScoreKeeper scoreKeeper;
    ShowScore showScore;

    UIDisplay uiDisplay;
    LevelManager levelManager;

    PowerUpDispenser powerUpDispenser;

    int maxHealth;

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        uiDisplay = FindObjectOfType<UIDisplay>();
        levelManager = FindObjectOfType<LevelManager>();
        showScore = FindObjectOfType<ShowScore>();
        powerUpDispenser = GetComponent<PowerUpDispenser>();
        maxHealth = health;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            TakeDamage(damageDealer.GetDamage());
            PlayHitEffect();
            PlayExplotionSFX();
            ShakeCamera();
            damageDealer.Hit();
        }
        if (!isPlayer) return;
        if (other.gameObject.tag == "HealthPickUp")
        {
            int healthAmount = other.GetComponent<HealthPickUp>().GetHealthAmount();
            AddHealth(healthAmount);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isPlayer)
        {
            if (levelManager == null) return;
            levelManager.LoadGameOver();
        }
        else
        {
            if (showScore != null) showScore.show(score);
            scoreKeeper.AddToCurrentScore(score);
            powerUpDispenser.OnEnemyDestroyed();
        }
        Destroy(gameObject);
    }

    void PlayHitEffect()
    {
        if (hitEffect == null) return;

        ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }

    void PlayExplotionSFX()
    {
        if (audioPlayer == null) return;

        audioPlayer.PlayExplotionClip();
    }

    void ShakeCamera()
    {
        if (cameraShake != null && applyCameraShake)
        {
            cameraShake.Play();
        }
    }

    public int GetHealth() => health;

    public void AddHealth(int amount)
    {
        if (!isPlayer) return;
        
        if (health + amount > maxHealth) {
            health = maxHealth;
        } else {
            health += amount;
        }
    }

}
