using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class AIMovement : MonoBehaviour
{
	[SerializeField] private bool inRange;
	//[SerializeField] 
	public bool inAttackAI;
	public Transform player; // Reference to the player character
	private NavMeshAgent navMeshAgent;
	private Animator animator;
	private bool isMoving;
	private bool turnAgain;
	private bool chase;
	public float attackDist = 15.0f;
	public int damage = 10;  // Damage value to apply to the player
	public PlayerMovement playerMovement;

	void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		isMoving = false;
		inRange = false;
		inAttackAI = false;
		turnAgain = true;
		chase = false;
	}



	void Update()
	{
		//Debug.Log(inRange);
		float distance = Vector3.Distance(player.position, transform.position);
		if (distance < attackDist || chase) // Adjust the distance threshold
		{
			// Move towards the player
			navMeshAgent.SetDestination(player.position);
			isMoving = true;
			chase = true;
		}
		else
		{
			if (isMoving)
			{
				// Stop the AI character when it reaches the player
				navMeshAgent.isStopped = true;
				isMoving = false;
				turnAgain = true;
			}
			// Rotate to face the player
			Vector3 lookDirection = (player.position - transform.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
		}

		if (distance < 3.0f)
		{
			inRange = true;
			//Debug.Log("AI ATTACK");
			inAttackAI = true;
			StartCoroutine(Attack());
			if (turnAgain == true)
			{
				Vector3 lookDirection = (player.position - transform.position).normalized;
				Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
				turnAgain = false;

			}
		}


		animator.SetFloat("Speed", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
		//bool inAttackPlayer = playerMovement.inAttackPlayer;
		//Debug.Log(inAttackPlayer);
	}

	private IEnumerator Attack()
	{
		//Debug.Log("there");
		inAttackAI = true;
		animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
		animator.SetTrigger("Attack");

		yield return new WaitForSeconds(3.0f);
		animator.SetTrigger("Idle");
		animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
		yield return new WaitForSeconds(3.0f);
		inAttackAI = false;


	}


	private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is an enemy
        if (other.CompareTag("PlayerAxe"))
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