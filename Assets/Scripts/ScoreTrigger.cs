using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script defines behavior of objects (Coins) that trigger score updates when collided with the player
public class ScoreTrigger : MonoBehaviour
{

    [Tooltip("The score value that will be added to the overall score when player is collided with special trigger (Coin)")]
    [SerializeField] private int scoreValue = 1;

    // A reference to the GameManager script in the scene
    private GameManager gameManager;

    /// <summary>
    /// Awake metod is called before the Start method
    /// </summary>
    private void Awake()
    {
        // Find and store a reference to the GameManager script in the scene
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Called when this object's collider is triggered by another collider
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // If the other collider's tag is "Player"
        if (other.CompareTag("Player") == true)
        {
            // Call the GameManager's ScoreModifier function with the score value of this object
            gameManager.ScoreModifier(scoreValue);

            // Deactivate this object to prevent further collisions
            gameObject.SetActive(false);
        }
    }

}
