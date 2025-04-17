using UnityEngine;

public class StraightChaser : BaseCreature
{
    [Header("StraightChaser Specifics")]
    public float speedMultiplier = 1.5f;
    // public int damage = 1; // Damage is now handled by BaseCreature, but you could override it here if needed
    // public int hitsToKill = 3; // Player health handles this
    // private int currentHits = 0; // Player health handles this

    protected override void Start()
    {
        base.Start(); 
    }


    protected override void Update()
    {
        // Need to check player exists before using its transform
        if (player == null)
        {
            base.Update(); // Fallback to base wandering or idle if player is gone
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < detectionRange)
        {
            // --- StraightChaser's Custom Movement Logic ---
            Vector2 direction = (player.transform.position - transform.position).normalized;
            float currentSpeed = moveSpeed;
            // Apply speed multiplier if moving mostly along an axis
            if (Mathf.Abs(direction.x) < 0.1f || Mathf.Abs(direction.y) < 0.1f)
            {
                currentSpeed *= speedMultiplier;
            }
            movement = direction; // Update movement direction
            // Apply velocity directly here, overriding BaseCreature's ApplyMovement for chase behavior
             if (rb != null)
             {
                rb.linearVelocity = movement * currentSpeed;
             }

        }
        else
        {
            // Player out of range, use BaseCreature's Update logic (wandering)
            base.Update();
        }
    }

    // Damage is now handled by BaseCreature.OnTriggerEnter2D.
    // You only need to override this if the StraightChaser should do something *else*
    // specific upon hitting the player (e.g., play a sound, trigger an effect).
    // If not needed, you can remove this override and the abstract method in BaseCreature.
    public override void OnPlayerCollision(GameObject player)
    {
        // Base class handles damage via OnTriggerEnter2D now.
        // Add any additional effects specific to StraightChaser collision here.
        Debug.Log("Straight Chaser specific OnPlayerCollision code executed (if needed).");
        // For example: PlayAttackAnimation(); ApplySlowEffect();
    }

     // Optional: You might want to override ApplyMovement if the base implementation interferes
     // protected override void ApplyMovement() { /* Do nothing here if Update handles velocity */ }
}