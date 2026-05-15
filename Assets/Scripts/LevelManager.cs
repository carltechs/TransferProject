using System.Collections;
using TMPro;
using UnityEngine;
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
    private int totalKills = 0;
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

    // Called when a tower kills an enemy
    public void AddKill()
    {
        totalKills++;
    }

    // Kept for spawner compatibility (does not add to kill count)
    public void EnemyDestroyed() { }

    public void TakeDamage(int damage)
    {
        if (gameEnded) return;

        hostHP -= damage;
        if (hpSlider != null) hpSlider.value = hostHP;

        if (hostHP <= 0)
        {
            hostHP = 0;
            GameOver();
        }
    }

    // --- VICTORY ---
    public void Victory()
    {
        if (gameEnded) return;
        gameEnded = true;
        Time.timeScale = 0f;

        // Award 3 skill points for winning
        if (SkillTreeManager.Instance != null)
            SkillTreeManager.Instance.AddSkillPoints(3);

        // Show results panel then load town scene
        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
            resultHeaderText.text = "Victory!";
            resultHeaderText.color = Color.green;
            killsText.text = "Viruses Neutralized: " + totalKills;
            hpText.text = "Final Host Integrity: " + hostHP + "%";

            // Wait a moment then load town
            StartCoroutine(LoadTownAfterDelay(2f));
        }
        else
        {
            LoadTownScene();
        }
    }

    private IEnumerator LoadTownAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        LoadTownScene();
    }

    private void LoadTownScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TownScene"); // Make sure this name matches your town scene
    }

    // --- GAME OVER (loss) ---
    private void GameOver()
    {
        gameEnded = true;
        Time.timeScale = 0f;

        if (resultsPanel != null)
        {
            resultsPanel.SetActive(true);
            resultHeaderText.text = "The host has been compromised!";
            resultHeaderText.color = Color.red;
            killsText.text = "Viruses Neutralized: " + totalKills;
            hpText.text = "Final Host Integrity: " + hostHP + "%";
        }
    }

    // UI Buttons (Replay / Home)
    public void ReplayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TownScene");
    }

    // --- CURRENCY ---
    public void IncreaseCurrency(int amount) { currency += amount; }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        ShowWarning("NOT ENOUGH CURRENCY!");
        return false;
    }

    public void ShowWarning(string message)
    {
        if (warningText == null) return;
        StopAllCoroutines();
        StartCoroutine(FlashWarning(message));
    }

    private IEnumerator FlashWarning(string message)
    {
        warningText.text = message;
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningText.gameObject.SetActive(false);
    }

    // Fast forward (optional)
    private bool isFastForward = false;
    public void ToggleFastForward()
    {
        isFastForward = !isFastForward;
        Time.timeScale = isFastForward ? 2f : 1f;
    }
}