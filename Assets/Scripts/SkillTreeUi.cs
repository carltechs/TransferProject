using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text pointsText;
    public TMP_Text damageText;
    public Button damageButton1;
    public Button damageButton2;
    public Button closeButton;

    private SkillTreeManager manager;

    void Start()
    {
        manager = SkillTreeManager.Instance;
        panel.SetActive(false);

        damageButton1.onClick.AddListener(() => { if (manager.UpgradeDamage(1)) UpdateUI(); });
        damageButton2.onClick.AddListener(() => { if (manager.UpgradeDamage(2)) UpdateUI(); });
        closeButton.onClick.AddListener(() => panel.SetActive(false));
    }

    public void Open()
    {
        panel.SetActive(true);
        UpdateUI();
    }

    void UpdateUI()
    {
        pointsText.text = $"Skill Points: {manager.skillPoints}";
        damageText.text = $"DMG Lv.{manager.damageLevel} (+{(manager.damageLevel * 1.5f):F1}%)";
        damageButton1.interactable = (manager.damageLevel == 0 && manager.skillPoints > 0);
        damageButton2.interactable = (manager.damageLevel == 1 && manager.skillPoints > 0);
    }
}