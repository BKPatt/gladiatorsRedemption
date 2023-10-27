using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class playerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health points
    private int currentHealth;  // Current health points
    public HealthBar healthBar;
    public DialogManager dialogManager;


    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        // Reduce health by the specified damage
        currentHealth -= damage;

        // Check if healthBar is not null before calling SetHealth
        if (healthBar != null)
        {
            if (currentHealth >= 0)
            {
                healthBar.SetHealth(currentHealth);
            }
        }
        else
        {
            Debug.LogError("healthBar is not initialized");
        }

        if (currentHealth <= 0)
        {
            Die(); // If health is zero or less, the hero dies
        }
    }


    private void Die()
    {
        
    }
}