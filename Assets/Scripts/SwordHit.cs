using UnityEngine;

public class SwordHit : MonoBehaviour
{
    public int damage = 10;  // Damage value to apply to the enemy

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is an enemy
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Hit an enemy!");
            }
        }
    }
}