using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfigSO> waveConfigs;
    [SerializeField] float timeBetweenWaves = 0f;
    [SerializeField] bool isLooping = false;

    [SerializeField] int startingWave = 0;

    int currentWaveIndex = 0;

    WaveConfigSO currentWave;

    private void Awake() {
        if (startingWave > waveConfigs.Count || startingWave < 0) {
            throw new UnityException("Starting wave must be between 0 and " + waveConfigs.Count);
        }
    }

    public void StartSpawner()
    {
        StartCoroutine(SpawnEnemyWaves());
    }

    public WaveConfigSO getCurrentWare() => currentWave;
    IEnumerator SpawnEnemyWaves()
    {
        do
        {
            foreach (WaveConfigSO wave in waveConfigs)
            {
                currentWaveIndex++;
                if (currentWaveIndex < startingWave) continue;
                
                currentWave = wave;
                for (int i = 0; i < currentWave.GetEnemyCount(); i++)
                {
                    Instantiate(currentWave.GetEnemyPrefab(i),
                            currentWave.GetStartingWaypoint().position,
                            Quaternion.identity,
                            transform);
                    yield return new WaitForSeconds(currentWave.GetRandomSpawnTime());
                }
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        } 
        while (isLooping);
    }
}
