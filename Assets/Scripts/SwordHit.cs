using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public int damage = 10;  // Damage value to apply to the enemy
    public PlayerMovement playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is an enemy
        if (other.CompareTag("Enemy"))
        {
			bool inAttackPlayer = playerMovement.inAttackPlayer;
			Debug.Log(inAttackPlayer);
			
            if (inAttackPlayer == true)
            {
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    Debug.Log("Hit by player!");
                }
            }
        }
    }
}