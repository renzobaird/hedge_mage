using UnityEngine;

public class StraightChaser : BaseCreature
{
    [Header("StraightChaser Specifics")]
    public float speedMultiplier = 1.5f;
    

    protected override void Start()
    {
        base.Start();
        damage = 25; // This ensures the correct value is applied for this creature
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

    
    public override void OnPlayerCollision(GameObject player)
    {
        
        Debug.Log("Straight Chaser specific OnPlayerCollision code executed (if needed).");
        
    }

}