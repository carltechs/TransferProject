using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager Instance;

    public float damageStep = 0.015f; // 1.5%
    public int damageLevel = 0;       // 0, 1, or 2
    public float totalDamageMultiplier = 1f;
    public int skillPoints = 0;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); Load(); }
        else Destroy(gameObject);
    }

    public bool UpgradeDamage(int tier) // tier 1 or 2
    {
        if (damageLevel + 1 != tier || skillPoints < 1) return false;
        damageLevel = tier;
        skillPoints--;
        totalDamageMultiplier = 1f + (damageLevel * damageStep);
        Save();
        return true;
    }

    public void AddSkillPoints(int amount) { skillPoints += amount; Save(); }

    void Save()
    {
        PlayerPrefs.SetInt("DamageLevel", damageLevel);
        PlayerPrefs.SetInt("SkillPoints", skillPoints);
        PlayerPrefs.Save();
    }

    void Load()
    {
        damageLevel = PlayerPrefs.GetInt("DamageLevel", 0);
        skillPoints = PlayerPrefs.GetInt("SkillPoints", 0);
        totalDamageMultiplier = 1f + (damageLevel * damageStep);
    }
}