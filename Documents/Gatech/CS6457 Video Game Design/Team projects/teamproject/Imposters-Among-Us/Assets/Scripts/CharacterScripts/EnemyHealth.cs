using UnityEngine;

public class EnemyHealth : MonoBehaviour
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
        //Debug.Log(currentHealth + "Damage Left");
        if (currentHealth >= 0)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die(); // If health is zero or less, the enemy dies
        }
    }

    private void Die()
    {
        // Handle enemy death logic, such as playing death animations or removing the enemy from the scene
        Destroy(gameObject);
        dialogManager.StartScene("Quintus", 0);
    }
}