using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
    // Damage amount to subtract from the player's health.
    public int damage = 10;
    public AIMovement aiMovement;

    // Called when the Collider of the axe enters a trigger Collider (the player).
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with a GameObject tagged as "Player."
        if (other.CompareTag("Player"))
        {
            bool inAttackAI = aiMovement.inAttackAI;

            if (inAttackAI == true)
            {
                playerHealth PlayerHealth = other.GetComponent<playerHealth>();
                if (PlayerHealth != null)
                {
                    PlayerHealth.TakeDamage(damage);
                    Debug.Log("Hit the player :(!");
                }
            }
        }
    }
}

