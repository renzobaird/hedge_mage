using UnityEngine;

public class StraightChaser : BaseCreature
{
    private Vector2 lastValidDirection;
    private float damageCooldown = 1.5f;
    private float lastDamageTime = -Mathf.Infinity;
    private Animator animator;

    // Add a variable to cache the PlayerHealth component
    private PlayerHealth playerHealth;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("StraightChaser: Rigidbody2D not found!");

        movement = GetRandomDirection();
        lastValidDirection = movement;
        lastDamageTime = -damageCooldown;

        // Attempt to get the PlayerHealth component at the start if player exists
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                Debug.LogWarning($"StraightChaser: Player GameObject found, but it doesn't have a PlayerHealth component.");
            }
        }
    }

    protected override void Update()
    {
        // --- Modification Starts Here ---

        // 1. Ensure we have the player and PlayerHealth references
        // (Could also re-find player if null, depending on game design)
        if (player == null)
        {
            Wander(); // No player in scene, just wander
            return;
        }
        // If we didn't get playerHealth in Start, try again (optional, good if player spawns later)
        if (playerHealth == null)
        {
             playerHealth = player.GetComponent<PlayerHealth>();
             if (playerHealth == null) {
                  Wander(); // Player exists but no health component? Wander.
                  return;
             }
        }

        // 2. Check if the player is dead
        if (playerHealth.IsDead)
        {
            Wander(); // Player is dead, revert to wandering behavior
        }
        else // Player exists, has health component, and is NOT dead
        {
            // Original chase/wander logic based on distance
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < detectionRange)
            {
                // Chase alive player
                movement = (player.transform.position - transform.position).normalized;
                lastValidDirection = movement;
            }
            else
            {
                // Player is alive but too far away
                Wander();
            }
        }
        // --- Modification Ends Here ---

        // Note: ApplyMovement() call moved to FixedUpdate in the previous step

        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            movement.y = 0;
        }
        else
        {
            movement.x = 0;
        }

        if (animator != null)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
    }

    // FixedUpdate remains the same - applies the 'movement' vector calculated in Update
    protected virtual void FixedUpdate()
    {
        ApplyMovement();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // No changes needed here. If the player is dead, playerHealth.TakeDamage
            // will likely do nothing anyway due to its internal checks.
            // The chaser's own cooldown prevents spamming damage attempts.
            if (playerHealth != null && Time.time >= lastDamageTime + damageCooldown)
            {
                playerHealth.TakeDamage(damage);
                lastDamageTime = Time.time;
                Debug.Log($"{gameObject.name} hit player. Attempting {damage} damage.");
            }
        }
        else
        {
            movement = GetRandomDirection();
            lastValidDirection = movement;
        }
    }

    protected override void Wander()
    {
        // Keep wandering logic simple for now
        if (Random.Range(0f, 100f) < 1f)
        {
            movement = GetRandomDirection();
            lastValidDirection = movement;
        }
        else
        {
             movement = lastValidDirection;
        }
        // IMPORTANT: Ensure wander doesn't accidentally use player position
        // This implementation seems safe as it uses random/last direction.
    }

    private Vector2 GetRandomDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        return directions[Random.Range(0, directions.Length)];
    }

    protected override void ApplyMovement()
    {
        if (rb != null)
        {
            // --- Potential Improvement: Stop applying velocity if wandering into dead player ---
            // This is trickier, requires knowing *what* we are wandering into.
            // For now, just applying calculated movement. If player is dead, 'movement'
            // should be from Wander(), not towards player.

             // Check if player is dead AND movement is non-zero (still wandering)
             if (playerHealth != null && playerHealth.IsDead && movement != Vector2.zero) {
                 // Optional: Dampen or stop velocity if wandering near dead player
                 // This requires more complex checks (e.g., raycasting in wander direction)
                 // Simplest fix is just letting Wander() dictate movement.
             }

             rb.linearVelocity = movement.normalized * moveSpeed;
        }
    }
}