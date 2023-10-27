using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Public variables for movement speeds and interaction settings
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed = 700.0f;
    public float proximityRadius = 1.5f;
    public LayerMask detectableObjects;
    public GameObject interactUI;
    public GameObject currentInterlocutor;
    public int damage = 1;

    // Private variables for internal state and logic
    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    public bool inAttackPlayer;
    [SerializeField] private bool inAir;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    // Components
    private CharacterController controller;
    private Animator animator;
    public DialogManager dialogManager;

    // Booleans to check certain events
    private bool isFirstGlimpseTriggered = false;
    private bool draxusDialogueStarted = false;
    public AIMovement aiMovement;

    // Initialization
    private void Start()
    {
        // Initialize components
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inAttackPlayer = false;
    }

    // Main update loop
    private void Update()
    {
        // If in dialogue, disable movement and interactions
        if (dialogManager.isInDialogue)
        {
            Idle();
            FaceInterlocutor(GameObject.Find(dialogManager.currentNPC));
            interactUI.SetActive(false);
            interactUI.transform.parent.gameObject.SetActive(false);

            // Disable interaction UI text
            Text uiText = interactUI.GetComponentInChildren<Text>(true);
            if (uiText != null)
            {
                uiText.gameObject.SetActive(false);
            }

            // Make NPC face player during dialogue
            GameObject npc = GameObject.Find(dialogManager.currentNPC);
            if (npc != null)
            {
                CharacterAI npcAI = npc.GetComponent<CharacterAI>();
                if (npcAI != null)
                {
                    if (dialogManager.isInDialogue)
                    {
                        npcAI.SetDestination(transform);
                        FaceInterlocutor(npc);
                    }
                    else
                    {
                        npcAI.SetDestination(null);
                    }
                }
            }

            return;
        }

        // Normal gameplay logic
        Move();
        CheckProximity();
        CheckForInteractions();

        // Handle player attack with left mouse click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            inAttackPlayer = true;
            StartCoroutine(Attack());
        }
    }

    // Make the player face the interlocutor during dialogue
    private void FaceInterlocutor(GameObject interlocutor)
    {
        if (interlocutor == null)
            return;

        Vector3 directionToInterlocutor = (interlocutor.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToInterlocutor.x, 0, directionToInterlocutor.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Check if any interactable objects are in proximity
    private void CheckProximity()
    {
        bool interactableInRange = false;
        float draxusRange = 4.0f;  // Draxus-specific interaction range

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, proximityRadius, detectableObjects);

        // Special check for Draxus character
        CheckForDraxus(draxusRange);

        // Check for other interactable objects or NPCs
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("NPC") || hitCollider.GetComponent<DoorwayToTrainingRoom>() != null || hitCollider.GetComponent<DoorController>() != null)
            {
                interactableInRange = true;
                break;
            }
        }

        // Set interaction UI based on whether an interactable object is in range
        interactUI.SetActive(interactableInRange);
        interactUI.transform.parent.gameObject.SetActive(true);
        if (interactableInRange)
        {
            Text uiText = interactUI.GetComponentInChildren<Text>(true);
            if (uiText != null)
            {
                uiText.text = "Press E to Interact";
                uiText.gameObject.SetActive(true);
            }
        }
        else
        {
            interactUI.transform.parent.gameObject.SetActive(false);
        }
    }

    // Special proximity check for the Draxus character
    private void CheckForDraxus(float draxusRange)
    {
        Collider[] draxusColliders = Physics.OverlapSphere(transform.position, draxusRange, detectableObjects);
        foreach (var hitCollider in draxusColliders)
        {
            if (hitCollider.CompareTag("NPC") && !draxusDialogueStarted && hitCollider.gameObject.name == "Draxus")
            {
                if (dialogManager.currentSceneIndex == 0)
                {
                    dialogManager.StartDialogue("Draxus");
                    draxusDialogueStarted = true;
                }
            }
        }
    }

    // Handle all types of player movement
    private void Move()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        // Check if in the air
        if (inAir)
        {
            isGrounded = false;
        }

        // Reset vertical velocity if grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Handle player rotation
        float turn = Input.GetAxis("Mouse X");
        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);

        // Get movement directions
        float strafe = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Transform movement directions based on player's orientation
        moveDirection = new Vector3(strafe, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        // Handle walking, running, and idle states
        if (true)
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            moveDirection *= moveSpeed;

            // Handle jumping
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                StartCoroutine(Jump());
                isGrounded = false;
            }
        }

        // Apply the movement
        controller.Move(moveDirection * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Set player state to idle
    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    // Set player state to walking
    private void Walk()
    {
        moveSpeed = walkSpeed;
        EventManager.TriggerEvent<WalkEvent, Vector3>(new Vector3());
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    // Set player state to running
    private void Run()
    {
        moveSpeed = runSpeed;
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }

    // Coroutine for performing a jump
    private IEnumerator Jump()
    {
        if (isGrounded == true)
        {
            isGrounded = false;
            inAir = true;
            animator.SetLayerWeight(animator.GetLayerIndex("Jump Layer"), 1);
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

            yield return new WaitForSeconds(2.0f);
            isGrounded = false;
            animator.SetTrigger("Idle");
            animator.SetLayerWeight(animator.GetLayerIndex("Jump Layer"), 0);
        }
        inAir = false;
    }

    // Coroutine for performing an attack
    private IEnumerator Attack()
    {
        inAttackPlayer = true;
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1.3f);
        animator.SetTrigger("Idle");
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
        inAttackPlayer = false;
    }

    // Check for objects that the player can interact with
    private void CheckForInteractions()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Handle first-time interactions based on the current scene
        if (!isFirstGlimpseTriggered)
        {
            if (currentSceneName == "Cell")
            {
                isFirstGlimpseTriggered = true;
                dialogManager.StartDialogue("FirstGlimpse");
            }
            else if (currentSceneName == "TrainingRoom")
            {
                isFirstGlimpseTriggered = true;
                dialogManager.StartDialogue("Caelia");
            }
            else if (currentSceneName == "Colosseum")
            {
                isFirstGlimpseTriggered = true;
                dialogManager.StartDialogue("Opponent");
            }
        }

        // Check for interactable objects or NPCs in proximity
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, proximityRadius, detectableObjects);
        foreach (var hitCollider in hitColliders)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DoorController door = hitCollider.GetComponent<DoorController>();
                if (door != null)
                {
                    door.Interact();
                }

                // Handle scene transitions
                if (hitCollider.CompareTag("MoveScene"))
                {
                    if (currentSceneName == "Cell")
                    {
                        SceneManager.LoadScene("TrainingRoom");
                    }
                    else if (currentSceneName == "TrainingRoom")
                    {
                        SceneManager.LoadScene("Colosseum");
                    }
                    else if (currentSceneName == "Colosseum")
                    {
                        SceneManager.LoadScene("Cell");
                    }

                    return;
                }

                // Handle NPC interactions
                if (hitCollider.CompareTag("NPC"))
                {
                    string characterName = hitCollider.gameObject.name;
                    if (characterName == "Lucius")
                    {
                        dialogManager.StartScene("Lucius", 5);
                    }
                    else if (characterName == "Chiron")
                    {
                        dialogManager.StartScene("Chiron", 1);
                    }
                    else if (characterName == "Draxus")
                    {
                        dialogManager.StartScene("Draxus", 4);
                    }
                    else if (characterName == "Caelia")
                    {
                        dialogManager.StartScene("Caelia", 4);
                    }
                }
            }
        }
    }
}
