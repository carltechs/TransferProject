using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private Slider hpSlider;

    [Header("Results UI Panel")]
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private TextMeshProUGUI resultHeaderText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI hpText;

    [Header("Attributes")]
    public int currency;
    private int totalKills = 0; // This should only increase when a tower kills an enemy
    private int hostHP = 100;
    private bool gameEnded = false;

    private void Awake()
    {
        main = this;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        currency = 100;

        if (hpSlider != null)
        {
            hpSlider.maxValue = 100;
            hpSlider.value = hostHP;
        }

        if (warningText != null) warningText.gameObject.SetActive(false);
        if (resultsPanel != null) resultsPanel.SetActive(false);
    }

    // --- RESEARCH DATA & COMBAT ---

    // NEW FUNCTION: Call this ONLY when a tower kills an enemy
    public void AddKill()
    {
        totalKills++;
    }

    // This stays for the Spawner's wave logic, but we REMOVED totalKills++ from here
    public void EnemyDestroyed()
    {
        // We just keep this here so scripts don't break, 
        // but it no longer adds to your "Neutralized" score.
    }

    public void TakeDamage(int damage)
    {
        if (gameEnded) return;

        hostHP -= damage;

        if (hpSlider != null) hpSlider.value = hostHP;

        if (hostHP <= 0)
        {
            hostHP = 0;
            EndGame(false);
        }
    }

    public void Victory()
    {
        EndGame(true);
    }

    private void EndGame(bool isVictory)
    {
        gameEnded = true;
        Time.timeScale = 0f;

        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
            if (isVictory)
            {
                resultHeaderText.text = "Yey! You protected the host!";
                resultHeaderText.color = Color.green;
            }
            else
            {
                resultHeaderText.text = "The host has been compromised!";
                resultHeaderText.color = Color.red;
            }
            // Displays the accurate count from AddKill()
            killsText.text = "Viruses Neutralized: " + totalKills;
            hpText.text = "Final Host Integrity: " + hostHP + "%";
        }
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoHome()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // --- CURRENCY & WARNINGS ---
    public void IncreaseCurrency(int amount) { currency += amount; }
    public bool SpendCurrency(int amount)
    {
        if (amount <= currency) { currency -= amount; return true; }
        ShowWarning("NOT ENOUGH CURRENCY!");
        return false;
    }

    public void ShowWarning(string message)
    {
        if (warningText == null) return;
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(FlashWarning(message));
        }
    }

    private IEnumerator FlashWarning(string message)
    {
        warningText.text = message;
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningText.gameObject.SetActive(false);
    }

    private bool isFastForward = false;

    public void ToggleFastForward()
    {
        isFastForward = !isFastForward;

        if (isFastForward)
        {
            Time.timeScale = 2f; // 2x Speed
        }
        else
        {
            Time.timeScale = 1f; // Normal Speed
        }
    }
}