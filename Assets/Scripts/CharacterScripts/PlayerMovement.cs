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
    public float turnSpeed = 100.00f;
    public float proximityRadius = 1.5f;
    public LayerMask detectableObjects;
    public GameObject interactUI;
    public GameObject currentInterlocutor;
    public int damage = 1;
    public GameObject battleAxe = null;
    public GameObject halberd = null;
    public GameObject sword = null;

    // Private variables for internal state and logic
    private Vector3 moveDirection;
    private Vector3 velocity;
    Quaternion MyQuaternion;

    private float aLastTapTime = 0f;
    private int aTapCount = 0;
    private float dLastTapTime = 0f;
    private int dTapCount = 0;
    private float sLastTapTime = 0f;
    private int sTapCount = 0;
    private float doubleTapThreshold = 1.25f;

    [SerializeField] private bool isGrounded;
    public bool inAttackPlayer;
    [SerializeField] private bool inAir;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private GameObject dummyPanel = null;

    // Components
    private CharacterController controller;
    private Animator animator;
    public DialogManager dialogManager;

    // Booleans to check certain events
    private bool isFirstGlimpseTriggered = false;
    private bool draxusDialogueStarted = false;
    public AIMovement aiMovement = null;

    // Initialization
    private void Start()
    {
        // Initialize components
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        inAttackPlayer = false;

        if (SceneManager.GetActiveScene().name == "Colosseum")
        {
            var weapon = PlayerPrefs.GetString("weapon");

            switch (weapon)
            {
                case "Minotaur":
                    battleAxe.SetActive(true);
                    break;
                case "Dimachaeru":
                    halberd.SetActive(true);
                    break;
                case "Sagittarii":
                    sword.SetActive(true);
                    break;
                default:
                    // if nothing matches
                    halberd.SetActive(true);
                    break;
            }
        }
    }

    // Main update loop
    private void Update()
    {
        // If in dialogue, disable movement and interactions
        if (dialogManager.isInDialogue)
        {
            Idle();
            Cursor.lockState = CursorLockMode.None;
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
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            if ((Time.time - aLastTapTime) < doubleTapThreshold)
            {
                aTapCount++;

                if (aTapCount == 2)
                {
                    animator.SetTrigger("RollLeft");
                    StartCoroutine(RollMoveLeft());
                    aTapCount = 0;
                }
            }
            else
            {
                aTapCount = 1;
            }

            aLastTapTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if ((Time.time - dLastTapTime) < doubleTapThreshold)
            {
                dTapCount++;

                if (dTapCount == 2)
                {
                    animator.SetTrigger("RollRight");
                    StartCoroutine(RollMoveRight());
                    dTapCount = 0;
                }
            }
            else
            {
                dTapCount = 1;
            }

            dLastTapTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if ((Time.time - sLastTapTime) < doubleTapThreshold)
            {
                sTapCount++;

                if (sTapCount == 2)
                {
                    animator.SetTrigger("RollBack");
                    StartCoroutine(RollMoveBack());
                    sTapCount = 0;
                }
            }
            else
            {
                sTapCount = 1;
            }

            sLastTapTime = Time.time;
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
        bool dummyInRange = false;
        float draxusRange = 4.0f;  // Draxus-specific interaction range
        float dummyProximityRadius = 4.0f; // Specific proximity radius for dummy objects

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, proximityRadius, detectableObjects);
        Collider[] dummyHitColliders = Physics.OverlapSphere(transform.position, dummyProximityRadius, detectableObjects);

        // Special check for Draxus character
        CheckForDraxus(draxusRange);

        // Check for other interactable objects or NPCs
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("NPC") || (hitCollider.GetComponent<DoorwayToTrainingRoom>() != null && dialogManager.playerClan != "") || hitCollider.GetComponent<DoorController>() != null)
            {
                interactableInRange = true;
                break;
            }
        }

        // Check for dummy objects in proximity
        foreach (var hitCollider in dummyHitColliders)
        {
            if (hitCollider.CompareTag("dummy"))
            {
                dummyInRange = true;
                break;
            }
        }

        // Set interaction UI based on whether an interactable object is in range
        interactUI.SetActive(interactableInRange);
        interactUI.transform.parent.gameObject.SetActive(interactableInRange);
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

        // Show or hide the dummy panel based on proximity
        if (dummyPanel != null)
        {
            dummyPanel.SetActive(dummyInRange);
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
        if (inAir || inAttackPlayer) return;

        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        // Apply gravity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Handle player rotation based on mouse movement
        float turn = Input.GetAxis("Mouse X");
        transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);

        // Get input for movement
        float strafe = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Determine if the player is moving backward
        bool isMovingBackward = moveZ < 0;

        // Convert the input into a movement direction
        moveDirection = new Vector3(strafe, 0, Mathf.Abs(moveZ));
        moveDirection = transform.TransformDirection(moveDirection);

        // If moving backward, reverse the direction
        if (isMovingBackward)
        {
            moveDirection *= -1;
        }

        // Handle different movement states
        if (moveDirection != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift) && !isMovingBackward)
            {
                Run();
            }
            else
            {
                if (isMovingBackward && Input.GetKey(KeyCode.S))
                {
                    Backward();
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    Walk();
                }
            }
        }
        else
        {
            Idle();
        }

        // Apply movement speed
        moveDirection *= moveSpeed;

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if (Input.GetKey(KeyCode.A))
        {
            LeftTurn();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RightTurn();
        }
        else
        {
            animator.SetFloat("Strafe", 0.0f);
        }

        // Apply movement and gravity
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
        animator.SetFloat("Speed", 0.25f, 0.1f, Time.deltaTime);
    }

    // Set player state to running
    private void Run()
    {
        moveSpeed = runSpeed;
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);

        Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
        controller.Move(move);
    }

    private void Backward()
    {
        moveSpeed = -walkSpeed;
        EventManager.TriggerEvent<WalkEvent, Vector3>(new Vector3());
        animator.SetFloat("Speed", -0.5f, 0.1f, Time.deltaTime);
    }

    // Strafing to the right with a coroutine
    private void RightTurn()
    {
        animator.SetFloat("Strafe", 1.0f);
        StartCoroutine(StrafeCoroutine(transform.right));
    }

    // Strafing to the left with a coroutine
    private void LeftTurn()
    {
        animator.SetFloat("Strafe", -1.0f);
        StartCoroutine(StrafeCoroutine(-transform.right));
    }

    // Coroutine for turning
    private IEnumerator StrafeCoroutine(Vector3 direction)
    {
        float strafeDuration = 1.0f; // Duration of the strafe
        float strafeDistance = 5.0f; // Distance to strafe, adjust as needed

        float timer = 0;
        while (timer < strafeDuration)
        {
            moveDirection = direction * strafeDistance / strafeDuration;
            yield return null;
            timer += Time.deltaTime;
        }

        yield return new WaitForSeconds(strafeDuration);
        moveDirection = Vector3.zero;
    }

    // Coroutine for performing a jump
    private IEnumerator Jump()
    {
        if (isGrounded == true)
        {
            isGrounded = false;
            inAir = true;
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);

            yield return new WaitForSeconds(2.0f);
            isGrounded = true;
            inAir = false;  // Set inAir to false once the jump is completed
            animator.SetTrigger("Idle");
        }
    }

    private IEnumerator RollMoveLeft()
    {
        // Wait for 0.5 seconds before starting the roll movement
        yield return new WaitForSeconds(0.4f);

        float rollDuration = 1.0f; // Updated duration of the roll
        float rollDistance = 5f; // Distance to move during the roll
        Vector3 rollDirection = -transform.right; // Roll to the left

        float timer = 0;
        while (timer < rollDuration)
        {
            // Calculate how far to move this frame
            Vector3 displacement = rollDirection * (rollDistance / rollDuration) * Time.deltaTime;
            // Use CharacterController.Move for movement
            controller.Move(displacement);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RollMoveRight()
    {
        // Wait for 0.5 seconds before starting the roll movement
        yield return new WaitForSeconds(0.4f);

        float rollDuration = 1.0f; // Updated duration of the roll
        float rollDistance = 5f; // Distance to move during the roll
        Vector3 rollDirection = transform.right; // Roll to the right

        float timer = 0;
        while (timer < rollDuration)
        {
            // Calculate how far to move this frame
            Vector3 displacement = rollDirection * (rollDistance / rollDuration) * Time.deltaTime;
            // Use CharacterController.Move for movement
            controller.Move(displacement);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RollMoveBack()
    {
        // Wait for 0.5 seconds before starting the roll movement
        yield return new WaitForSeconds(0.4f);

        float rollDuration = 1.0f; // Updated duration of the roll
        float rollDistance = 5f; // Distance to move during the roll
        Vector3 rollDirection = -transform.forward; // Roll to the back

        float timer = 0;
        while (timer < rollDuration)
        {
            // Calculate how far to move this frame
            Vector3 displacement = rollDirection * (rollDistance / rollDuration) * Time.deltaTime;
            // Use CharacterController.Move for movement
            controller.Move(displacement);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    // Coroutine for performing an attack
    private IEnumerator Attack()
    {
        inAttackPlayer = true;
        float moveZ = Input.GetAxis("Vertical");
        moveDirection = new Vector3(0, 0, Mathf.Abs(moveZ));
        moveDirection = transform.TransformDirection(moveDirection);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(2.0f);
        //animator.SetTrigger("Idle");

        //Set the Quaternion rotation from the GameObject's position to the mouse position
        MyQuaternion.SetFromToRotation(transform.position, moveDirection);
        //Move the GameObject towards the mouse position
        transform.position = Vector3.Lerp(transform.position, moveDirection, turnSpeed * Time.deltaTime);
        //Rotate the GameObject towards the mouse position
        transform.rotation = MyQuaternion * transform.rotation;
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
                dialogManager.StartScene("Decimus", 1);
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
                    else if (currentSceneName == "TrainingRoom" && dialogManager.playerClan != "")
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
                    else if (characterName == "Decimus")
                    {
                        dialogManager.StartScene("Decimus", 0);
                    }
                    else if (characterName == "Minotaur Leader")
                    {
                        dialogManager.StartScene("Minotaur Leader", 0);
                    }
                    else if (characterName == "Dimachaeru Leader")
                    {
                        dialogManager.StartScene("Dimachaeru Leader", 0);
                    }
                    else if (characterName == "Sagittarii Leader")
                    {
                        dialogManager.StartScene("Sagittarii Leader", 0);
                    }
                }
            }
        }
    }
}
