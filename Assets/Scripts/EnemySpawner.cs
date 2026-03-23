using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro; // Added for your Wave Indicator

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private TextMeshProUGUI waveText; // Link your UI Text here

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private int maxWaves = 10; // Kept bob-jc's 10 waves, but you can change to 5 in Inspector

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Awake()
    {
        // Prevents double-counting if the scene reloads
        onEnemyDestroy.RemoveListener(EnemyDestroyed);
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void OnDestroy()
    {
        onEnemyDestroy.RemoveListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        UpdateWaveUI(); // Keeps your indicator updated every frame

        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        // Spawn logic: Check timer and if we have enemies left
        if (enemiesLeftToSpawn > 0 && timeSinceLastSpawn >= (1f / enemiesPerSecond))
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }

        // Check if wave is over
        if (enemiesLeftToSpawn == 0 && enemiesAlive <= 0)
        {
            EndWave();
        }
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave + " / " + maxWaves +
                            "\nEnemies: " + (enemiesLeftToSpawn + enemiesAlive);
        }
    }

    private IEnumerator StartWave()
    {
        isSpawning = false;
        yield return new WaitForSeconds(timeBetweenWaves);

        enemiesLeftToSpawn = EnemiesPerWave();
        enemiesAlive = 0;
        isSpawning = true;
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;

        if (currentWave >= maxWaves)
        {
            LevelCompleted();
            return;
        }

        currentWave++;
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index];

        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);

        enemiesLeftToSpawn--;
        enemiesAlive++;
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void LevelCompleted()
    {
        if (waveText != null) waveText.text = "SYSTEM SECURE!";
        Debug.Log("LEVEL CLEARED! You survived all " + maxWaves + " waves.");
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}