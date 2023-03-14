using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("Defines how many lives player will have at the start of the game")]
    [SerializeField] private int lives = 3;
    [Tooltip("Defines the time limit that the level must be completed within")]
    [SerializeField] float timeLimit = 60f;
    [Tooltip("Audio clip that plays when the score is modified")]
    [SerializeField] private AudioClip scoreSound;
    [Tooltip("Audio clip that plays when the HP is lost")]
    [SerializeField] private AudioClip loseLifeSound;
    [Tooltip("Audio clip that plays when level is passed")]
    [SerializeField] private AudioClip levelPassedSound;
    [Tooltip("Audio clip that plays when the time is out")]
    [SerializeField] private AudioClip timesUpSound;
    
    // defines the start amount of points
    private int currentScore = 0;

    // set level timer
    private float levelTimer = -1f;

    // initialize references to objects in scene
    private PlayerController player;
    private HUD hud;
    private AudioSource audioSource;
    private MovingPlatform[] movingPlatforms;

    /// <summary>
    /// Awake metod is called before the Start method
    /// </summary>
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        hud = FindObjectOfType<HUD>();
        movingPlatforms = FindObjectsOfType<MovingPlatform>();
        TryGetComponent(out audioSource);
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        levelTimer = timeLimit;
        hud.UpdateLives(lives);
    }

    /// <summary>
    /// Restart the current level
    /// </summary>
    static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Update the timer while it's still positive
        if (levelTimer > 0)
        {
            levelTimer -= Time.deltaTime;
            if (levelTimer <= 0)
            {
                GameOver();
                levelTimer = 0;
            }
            hud.UpdateTime(levelTimer);
        }
        // If the timer has reached zero, allow the player to restart the level
        else
        {
            if (Input.GetKeyDown(KeyCode.R) == true)
            {
                // reload the scene if time is up and "R" key is pressed
                Restart();
            }
        }
    }

    /// <summary>
    /// End the game and disable player movement
    /// </summary>
    private void GameOver()
    {
        Debug.Log("Game over");
        levelTimer = -1;
        player.CanMove = false;
        if (audioSource != null)
        {
            // Play "time's up" sound
            audioSource.PlayOneShot(timesUpSound);
        }
    }

    /// <summary>
    /// Reset the position of all moving platforms
    /// </summary>
    private void ResetLevel()
    {
        foreach (MovingPlatform platform in movingPlatforms)
        {
            platform.ResetPlatform();
        }
    }

    /// <summary>
    /// End the level and disable player movement
    /// </summary>
    public void LevelComplete()
    {
        Debug.Log("Level completed");
        levelTimer = -1;
        player.CanMove = false;

        if (audioSource != null)
        {
            // Play "level passed" sound
            audioSource.PlayOneShot(levelPassedSound);
        }
    }

    /// <summary>
    /// A method that modifies the current score by adding the given amount and updates the score display
    /// </summary>
    public void ScoreModifier(int amount)
    {
        // Make sure that the score is never negative
        currentScore += amount;
        
        if (currentScore < 0)
        {
            currentScore = 0;
        }
        // Update the score display
        hud.UpdateScoreDisplay(currentScore);

        // If an audio source is attached, play the score sound
        if (audioSource != null)
        {
            audioSource.PlayOneShot(scoreSound);
        }

        // Log the current score to the console
        Debug.Log("Current score: " + currentScore);
    }

    // A method that subtracts one life from the player and updates the lives display
    public bool SubstractLives()
    {
        if (lives > 0)
        {
            // Subtract one life and update HP display
            lives--;
            Debug.Log("Lives left: " + lives);
            hud.UpdateLives(lives);
            
            // Reset the level
            ResetLevel();
            if (audioSource != null)
            {
                // If an audio source is attached, play the "HP lose" sound
                audioSource.PlayOneShot(loseLifeSound);
            }
            return true;
        }
        
        else
        {
            // If the player has no more lives then trigger the game over event
            GameOver();
            return false;
        }
    }
}
