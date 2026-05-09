using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    private bool isDestroyed = false;
    [SerializeField] private int currencyWorth = 50;

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0 && !isDestroyed)
        {
            isDestroyed = true; // Set this first to prevent double-counting!

            // 1. Tell the LevelManager to count this as a neutralization
            LevelManager.main.AddKill();

            // 2. Tell the Spawner to update the wave count
            FindObjectOfType<EnemySpawner>().EnemyDestroyed();

            // 3. Give the player money
            LevelManager.main.IncreaseCurrency(currencyWorth);

            // 4. Remove the virus from the game
            Destroy(gameObject);
        }
    }
}