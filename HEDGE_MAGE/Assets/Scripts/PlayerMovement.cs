using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 movement;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private Animator animator;
    private bool canMove = true; // Added flag to control movement

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
        // Make sure to get the Animator component
        // If it might be on a child object, use GetComponentInChildren<Animator>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("PlayerMovement: Animator component not found on this GameObject.");
        }
    }

    void Update()
    {
        // Prioritize the dead check - if dead, clear movement and return
        if (playerHealth != null && playerHealth.IsDead)
        {
            movement = Vector2.zero; // Stop attempting to move

            // Update animator if it exists
            if (animator != null)
            {
                // Assuming you have a boolean parameter "isDead" in your Animator Controller
                animator.SetBool("isDead", true);
                 // Optionally reset movement parameters if you have them
                 // animator.SetFloat("Horizontal", 0f);
                 // animator.SetFloat("Vertical", 0f);
                 // animator.SetFloat("Speed", 0f);
            }
            return; // Exit Update early if dead
        }

        // If not dead, ensure the animator reflects that
        if (animator != null)
        {
            animator.SetBool("isDead", false);
        }

        // Only process input if movement is enabled
        if (!canMove)
        {
            movement = Vector2.zero; // Ensure movement is zero if not allowed
            // Optionally update animator movement parameters to idle here if needed
            // if (animator != null) { /* set animator movement params to 0 */ }
            return; // Don't process input if movement is disabled
        }

        // Get input for both WASD and arrow keys
        movement.x = Input.GetAxisRaw("Horizontal"); // Left/Right arrows and A/D
        movement.y = Input.GetAxisRaw("Vertical");   // Up/Down arrows and W/S

        // Optionally update animator based on movement input here
        // if (animator != null)
        // {
        //    animator.SetFloat("Horizontal", movement.x);
        //    animator.SetFloat("Vertical", movement.y);
        //    animator.SetFloat("Speed", movement.sqrMagnitude); // Use sqrMagnitude for efficiency
        // }
    }

    void FixedUpdate()
    {
        // Do not move if dead OR if movement is disabled
        if (!canMove || (playerHealth != null && playerHealth.IsDead))
        {
            // It might be good practice to ensure velocity is zeroed when not moving
             rb.linearVelocity = Vector2.zero;
            return;
        }

        // Apply movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime); // Normalize movement vector
    }

    // --- This is the new method to fix the errors ---
    /// <summary>
    /// Enables or disables player input processing and movement.
    /// </summary>
    /// <param name="enabled">True to enable movement, false to disable.</param>
    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        // If disabling movement, immediately stop current movement/velocity
        if (!enabled)
        {
            movement = Vector2.zero;
            if (rb != null) // Check if rb is initialized
            {
                 rb.linearVelocity = Vector2.zero;
            }
             // Optionally update animator movement params to 0 here as well
             // if (animator != null) { /* set animator movement params to 0 */ }
        }
    }
}