using UnityEngine;
using System.Collections;

public class Dummy_Controller : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private bool isAnimationPlaying = false;
    public PlayerMovement PlayerMovement { get; private set; }
    [SerializeField] private Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        PlayerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update() 
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (PlayerMovement.inAttackPlayer && !isAnimationPlaying && distance < 3.0f)
        {
            StartCoroutine(PlayAnimationAndWait());
        }
    }

    IEnumerator PlayAnimationAndWait()
    {
        // yield return new WaitForSeconds(0.5f);
        animator.Play("New State");
        yield return new WaitForSeconds(1.5f);
        isAnimationPlaying = true;
        animator.Play("GetHitByPlayer");
        yield return new WaitForSeconds(4.8f);
        animator.speed = 0.0f;
        animator.Play("New State");
        animator.speed = 1.0f;
        isAnimationPlaying = false;
    }
}
