using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Define variables to control camera movement sensitivity and drag
    [Tooltip("Controls how much the camera will move in response to detected mouse input.")]
    [SerializeField] private float sensitivity = 2f;

    [Tooltip("Controls how much the camera will continue to move after mouse input ceases.")]
    [SerializeField] private float drag = 3f;


    // Define variables to hold mouse direction and values for smooth dragging
    private Vector2 mouseDir;
    private Vector2 smoothness;
    private Vector2 result;
    private Transform player;

    /// <summary>
    /// enable/disable ability to move the camera by moving the mouse
    /// </summary>
    public bool LookEnabled { get; set; } = true;

    /// <summary>
    /// Awake metod is called before the Start method
    /// </summary>
    void Awake()
    {
        // Set the player variable to the parent of the camera object
        player = transform.parent;

        // Enable camera movement by default
        LookEnabled = true;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Only update camera movement if it is enabled
        if (LookEnabled == true)
        {
            // Get the current mouse direction and multiply by sensitivity
            mouseDir = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            mouseDir *= sensitivity;

            // Smooth out the camera movement using Lerp
            smoothness = Vector2.Lerp(smoothness, mouseDir, 1 / drag);

            // Clamp the vertical camera movement to prevent flipping
            result += smoothness;
            result.y = Mathf.Clamp(result.y, -90, 90);

            // Rotate the player object horizontally based on the x direction of the camera movement
            player.rotation = Quaternion.AngleAxis(result.x, player.up);

            // Rotate the camera object vertically based on the y direction of the camera movement
            transform.localRotation = Quaternion.AngleAxis(-result.y, Vector3.right);
        }
    }
}
