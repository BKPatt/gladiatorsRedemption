using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class playerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health points
    private int currentHealth;  // Current health points
    public HealthBar healthBar;


    private void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        // Reduce health by the specified damage
        currentHealth -= damage;
        if (currentHealth >= 0)
        {
            healthBar.SetHealth(currentHealth);
        }
        //Debug.Log(currentHealth + "Damage Left");

        if (currentHealth <= 0)
        {
            Die(); // If health is zero or less, the hero dies
        }
    }

    private void Die()
    {
        // Handle enemy death logic, such as playing death animations or removing the enemy from the scene
        Destroy(gameObject); // This is a simple example; you may want to customize it.
    }
}