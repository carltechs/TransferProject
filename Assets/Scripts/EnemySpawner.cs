using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private int maxWaves = 5; // 1. Added a limit for your STEM project ending

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Awake()
    {
        // Clean up the listener first to prevent "ghost" double-counting
        onEnemyDestroy.RemoveListener(EnemyDestroyed);
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        // Spawn logic
        if (enemiesLeftToSpawn > 0 && timeSinceLastSpawn >= (1f / enemiesPerSecond))
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }

        // 2. CHECK FOR WAVE END: All spawned AND all dead
        if (enemiesLeftToSpawn == 0 && enemiesAlive <= 0)
        {
            EndWave();
        }
    }

    private IEnumerator StartWave()
    {
        isSpawning = false; // Stay quiet during the countdown
        yield return new WaitForSeconds(timeBetweenWaves);

        enemiesLeftToSpawn = EnemiesPerWave();
        enemiesAlive = 0; // Reset count for the new wave
        isSpawning = true;
    }

    private void EndWave()
    {
        isSpawning = false;

        // 3. Victory Condition
        if (currentWave >= maxWaves)
        {
            WinGame();
            return;
        }

        currentWave++;
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        // Randomize which virus prefab spawns for more variety!
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

    private void WinGame()
    {
        Debug.Log("LEVEL CLEAR: SYSTEM SECURE!");
        // Would you like me to help you show a "Victory" UI screen here?
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
}