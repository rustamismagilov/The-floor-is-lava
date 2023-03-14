using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Serialized class to store the point where the platform will be moving to and the amount of time the platform will wait for when it reaches this point
    [System.Serializable]
    public class PlatformTargetPoint
    {
        [Tooltip("The point where the platorm will be move to")]
        [SerializeField] private Transform point;
        [Tooltip("The amount of time the platform will wait for when it reaches this point")]
        [SerializeField] private float idleTime;

        public Transform Point { get { return point; } }
        public float IdleTime { get { return idleTime; } }
    }

    [Tooltip("Platform's moving speed")]
    [SerializeField] protected float platformMovingSpeed = 3f;
    [Tooltip("List of points between which the platform will be moving")]
    [SerializeField] protected PlatformTargetPoint[] points;

    // Index of the point in the PlatformTargetPoints array that the platform is currently moving towards
    protected int currentPointIndex = 0;

    // Timer used to wait at each PlatformTargetPoint
    protected float timer = -1;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    protected virtual void Start()
    {
        // Initialize platform's position and current target point
        ResetPlatform();
    }

    /// <summary>
    /// FixedUpdate is called at a fixed interval and is used for physics calculations
    /// </summary>
    private void FixedUpdate()
    {
        // If the timer is less than 0, the platform moves towards a target point
        if (timer < 0)
        {
            // move the platform towards the next point in the list
            transform.position += platformMovingSpeed * Time.deltaTime * (points[currentPointIndex].Point.position - transform.position).normalized;
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    protected virtual void Update()
    {
        // If timer is greater than or equal to 0, the platform is waiting at a PlatformTargetPoint
        if (timer >= 0)
        {
            // Increment the timer
            timer += Time.deltaTime;

            // If the timer is greater than or equal to the idle time at the current PlatformTargetPoint, reset the timer and move to the next target point
            if (timer >= points[currentPointIndex].IdleTime)
            {
                timer = -1;
                currentPointIndex++;

                // If the current target point is the last in the array, set the index to the first point
                if (currentPointIndex >= points.Length)
                {
                    currentPointIndex = 0;
                }
            }
        }

        // If the platform is moving towards a target point and has reached it, start the idle timer
        else if (Vector3.Distance(transform.position, points[currentPointIndex].Point.position) < 0.1f)
        {
            if (timer < 0)
            {
                timer = 0;
            }
        }

    }

    /// <summary>
    /// OnTriggerEnter is called when a collider enters the trigger zone of the platform
    /// </summary>
    protected virtual void OnTriggerEnter(Collider other)
    {
        // If the other object has the "Player" tag, then set the transform of the player as a child of this transform
        // This is useful for moving platforms that carry the player with them when they move

        if (other.CompareTag("Player") == true)
        {
            other.transform.SetParent(transform);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        // If the collider is a player, set the platform as the parent to ensure the player moves with the platform
        if (other.CompareTag("Player") == true)
        {
            // when the player exits the platform, set its parent object to null
            other.transform.SetParent(null);
        }
    }

    /// <summary>
    /// Resets the platform to the first point in the list
    /// </summary>
    public virtual void ResetPlatform()
    {
        // If the current target point index is greater than 0, set it to the previous index.
        if (currentPointIndex > 0)
        {
            currentPointIndex--;
        }

        // Otherwise, set the current target point index to the last index in the list.
        else
        {
            currentPointIndex = points.Length - 1;
        }

        // Set the platform's position to the new target point's position.
        transform.position = points[currentPointIndex].Point.position;
    }
}
