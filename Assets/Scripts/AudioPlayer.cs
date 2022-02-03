using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] AudioClip shootingClip;
    [SerializeField] [Range(0f, 1f)] float shootingVolume = 1f;

    [Header("Explotion")]
    [SerializeField] AudioClip explotionClip;
    [SerializeField] [Range(0f, 1f)] float explotionVolume = 1f;

    static AudioPlayer instance;
    
    private void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayShootingClip()
    {
        if (shootingClip == null) return;

        PlayClip(shootingClip, shootingVolume);
    }

    public void PlayExplotionClip()
    {
        if (explotionClip == null) return;
        PlayClip(explotionClip, explotionVolume);
    }

    private void PlayClip(AudioClip clip, float volume)
    {
        if (clip == null) return;

        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }
}
