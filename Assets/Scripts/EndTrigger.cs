using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    // Reference to the GameManager object
    private GameManager gameManager;

    /// <summary>
    /// Awake metod is called before the Start method
    /// </summary>
    private void Awake()
    {
        // Find the GameManager object in the scene and assign it to gameManager
        gameManager = FindObjectOfType<GameManager>();
    }

    // Called when the trigger collider collides with another collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other collider has the "Player" tag
        if (other.CompareTag("Player") == true)
        {
            // Call the LevelComplete() method on the GameManager object
            gameManager.LevelComplete();

            // Load the scene with index 1
            SceneManager.LoadScene(1);
        }
    }
}
