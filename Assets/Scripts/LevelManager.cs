using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 1. CRITICAL: Add this to use TextMeshPro!

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI warningText; // 2. Drag your TextMeshPro object here in the Inspector

    public int currency;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 100;
        // Hide the warning at the start
        if (warningText != null) warningText.gameObject.SetActive(false);
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            // 3. Trigger the visual warning instead of just a Debug.Log
            ShowWarning("NOT ENOUGH CURRENCY!");
            return false;
        }
    }

    // 4. The "Timer" logic to show and hide the message
    public void ShowWarning(string message)
    {
        if (warningText == null) return;

        StopAllCoroutines(); // Reset the timer if they spam the button
        StartCoroutine(FlashWarning(message));
    }

    private IEnumerator FlashWarning(string message)
    {
        warningText.text = message;
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f); // Show for 2 seconds

        warningText.gameObject.SetActive(false);
    }
}