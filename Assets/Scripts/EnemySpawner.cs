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
    [SerializeField] private int maxWaves = 10; 

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Awake()
    {
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
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }

        
        if (enemiesAlive <= 0 && enemiesLeftToSpawn <= 0)
        {
            EndWave();
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        enemiesLeftToSpawn = EnemiesPerWave();
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

    private int EnemiesPerWave()
    {
        
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private void LevelCompleted()
    {
        Debug.Log("LEVEL CLEARED! You survived all " + maxWaves + " waves.");
       
    }
}