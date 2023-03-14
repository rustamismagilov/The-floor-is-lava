using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    [Tooltip("The point where player will teleport if he touched the trigger")]
    [SerializeField] private Transform checkpoint;

    // initialize game manager instance
    private GameManager gameManager;

    /// <summary>
    /// Awake method is called before the Start method
    /// </summary>
    private void Awake()
    {
        // Finding the instance of the game manager
        gameManager = FindObjectOfType<GameManager>();
    }

    // This method will substract lives if player touches the lava
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the lava trigger has the tag "Player"
        if (other.CompareTag("Player") == true)
        {
            // If the player is entered the trigger, subtract a life from the player and then check if the player still has lives left =>
            // If the player has lives left, teleport the player to the checkpoint
            if (gameManager.SubstractLives() == true)
            {
                // Accessing the PlayerController component attached to the player and teleporting it to the checkpoint
                other.TryGetComponent(out PlayerController controller);
                controller.TeleportToPosition(checkpoint.position);
            }
        }
    }
}
