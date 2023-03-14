using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The gravity acting on the player character")]
    [SerializeField] private float gravity = 30f;
    [Tooltip("The force applied to the player character when jumping")]
    [SerializeField] private float jumpForce = 20f;
    [Tooltip("The default movement speed of the player character")]
    [SerializeField] private float defaultMovementSpeed = 3.5f;
    [Tooltip("The multiplier applied to the movement speed when sprinting")]
    [SerializeField] private float sprintMultiplier = 2f;
    [Tooltip("The sound played when the player jumps")]
    [SerializeField] private AudioClip jumpSound;
    [Tooltip("An array of sounds played when the player walks")]
    [SerializeField] private AudioClip[] footstepsSound;
    [Tooltip("The time between steps when walking")]
    [SerializeField] private float stepTime = 0.5f;
    [Tooltip("The time between steps when sprinting")]
    [SerializeField][Range(0.1f, 1f)] private float sprintStepTimeMultiplier = 0.5f;

    // The amount of motion to apply to the player character
    private Vector3 motionStep;

    // The base velocity of the player
    private float velocity = 0f;

    // The base speed of the player
    private float speed = 0f;

    // The character controller component attached to the "player" object
    private CharacterController controller;

    private float stepTimer = 0f;

    // The audio source component attached to the "player" object
    private AudioSource audioSource;

    /// <summary>
    /// enable/disable ability to move the player via mouse input
    /// </summary>
    public bool CanMove { get; set; } = true;

    /// <summary>
    /// Returns the sprint speed
    /// </summary>
    private float SprintSpeed { get { return defaultMovementSpeed * sprintMultiplier; } }

    /// <summary>
    /// Returns true if player is sprinting
    /// </summary>
    private bool Sprinting { get { return speed == SprintSpeed; } }

    /// <summary>
    /// Awake metod is called before the Start method
    /// </summary>
    private void Awake()
    {
        TryGetComponent(out controller);
        TryGetComponent(out audioSource);
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        speed = defaultMovementSpeed;
    }

    /// <summary>
    /// FixedUpdate may be called more than once per frame
    /// </summary>
    private void FixedUpdate()
    {
        // updates the velocity of the player controller based on gravity and whether it is grounded or not
        if (CanMove == true)
        {
            if (controller.isGrounded == true)
            {
                velocity = -gravity * Time.deltaTime;
            }
            else
            {
                velocity -= gravity * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (CanMove == true)
        {
            if (controller.isGrounded == true)
            {
                // Check for sprinting input and adjust speed accordingly
                if (Input.GetButtonUp("Sprint") == true)
                {
                    speed = defaultMovementSpeed;
                }
                else if (Input.GetButton("Sprint") == true)
                {
                    speed = SprintSpeed;
                }
                else if (Sprinting == true)
                {
                    speed = defaultMovementSpeed;
                }

                // Check for jumping input and apply jump force
                if (Input.GetButtonDown("Jump") == true)
                {
                    velocity = jumpForce;
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(jumpSound);
                    }
                }
            }
            // Apply movement to the "player" object
            ApplyMovements();
        }
    }

    /// <summary>
    /// This method applies movements based on player input and speed
    /// </summary>
    private void ApplyMovements()
    {
        // Set motionStep to zero before calculating new motion
        motionStep = Vector3.zero;

        // Add forward motion based on vertical input and right motion based on horizontal input
        motionStep += transform.forward * Input.GetAxisRaw("Vertical");
        motionStep += transform.right * Input.GetAxisRaw("Horizontal");

        // Scale motion by the speed and normalize it to prevent diagonal movement being faster
        motionStep = speed * motionStep.normalized;

        // Add velocity to the y-component of motionStep for vertical movement
        motionStep.y += velocity;

        // Move the controller by the motionStep scaled by time
        controller.Move(motionStep * Time.deltaTime);

        // Check if there is an audio source and the controller is grounded
        if (audioSource != null && controller.isGrounded == true)
        {
            // Check if the player is moving
            if (motionStep.z != 0 || motionStep.x != 0)
            {
                // Add the time passed to the step timer
                stepTimer += Time.deltaTime;

                // Check if the step timer has passed the step time or if the player is sprinting and the step timer has passed the sprint step time
                if (stepTimer >= stepTime || Sprinting == true && stepTimer > stepTime * sprintStepTimeMultiplier)
                {
                    // Reset the step timer
                    stepTimer = 0;

                    // Play a random footstep sound from an array of sounds
                    audioSource.PlayOneShot(footstepsSound[Random.Range(0, footstepsSound.Length)]);
                }
            }
        }
    }

    /// <summary>
    /// Teleports the object to a specified position
    /// </summary>
    /// <param name="position">The position to teleport to</param>
    public void TeleportToPosition(Vector3 position)
    {
        // Disable the controller to ensure the player doesn't collide with anything while teleporting
        controller.enabled = false;

        // Set the position of the player to the specified position
        transform.position = position;

        // Re-enable the controller to resume normal movement
        controller.enabled = true;
    }
}

