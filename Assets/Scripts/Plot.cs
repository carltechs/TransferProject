using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 1. ADD THIS AT THE TOP

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    // Prevents the "Highlight" from showing through the Shop UI
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        // 2. ADD THIS GUARD: Blocks the click if your finger is on the Shop
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (tower != null) return;

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild == null) return;

        // This calls your LevelManager. SpendCurrency already handles 
        // the "Not Enough Money" message now!
        if (LevelManager.main.SpendCurrency(towerToBuild.cost))
        {
            tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        }
    }
}