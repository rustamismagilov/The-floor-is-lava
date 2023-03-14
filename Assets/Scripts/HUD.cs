using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [Tooltip("Reference to the icon images in the HUD")]
    [SerializeField] private GameObject[] lifeIcons;
    [Tooltip("Reference to the timer text field in the HUD")]
    [SerializeField] private Text timerText;
    [Tooltip("Reference to the scores text field in the HUD")]
    [SerializeField] private Text scoreText;

    /// <summary>
    /// Toggles visibility and lock state of the mouse cursor
    /// </summary>
    public bool CursorEnabled
    {
        set
        {
            Cursor.visible = value;
            if (value == true)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    /// <summary>
    /// Updates score text in the HUD
    /// </summary>
    public void UpdateScoreDisplay(int score)
    {
        scoreText.text = "Score: " + score;
    }

    /// <summary>
    /// Updates amount of HP icons which are displayed based on the passed number of left lives
    /// </summary>
    /// <param name="lives"></param>
    public void UpdateLives(int lives)
    {
        // loop through all the life icons
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            // if the index is less than the amount of lives left, set the icon as active
            if (i < lives)
            {
                lifeIcons[i].SetActive(true);
            }
            else
            {
                lifeIcons[i].SetActive(false);
            }
        }

        // if there are more lives than icons, log a warning message
        if (lifeIcons.Length < lives)
        {
            Debug.LogWarning("There are less life icons than lives");
        }
    }


    /// <summary>
    /// Formats the passed time in stoprwatch format and updates the timer text in the HUD
    /// </summary>
    /// <param name="time"></param>
    public void UpdateTime(float time)
    {
        // calculate minutes and seconds based on the passed time
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.CeilToInt(time % 60);

        // if seconds are multiple of 60 and time is not 0, increment the minutes by 1 and set seconds to 0
        if (time > 0 && seconds % 60 == 0)
        {
            seconds = 0;
            minutes++;
        }

        // format the time string and update the timer text in the HUD
        string timeString = string.Format("Time left: {0:00}:{1:00}", minutes, seconds);
        timerText.text = timeString;
    }
}
