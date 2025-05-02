using UnityEngine;
using System.Collections.Generic;

public class StraightChaser : BaseCreature
{
    private Animator animator;
    private PlayerHealth playerHealth;
    private AStarGridManager pathfinder;

    private List<Vector3> path = new List<Vector3>();
    private int pathIndex = 0;

    private float repathInterval = 1f;
    private float nextRepathTime = 0f;

    private float damageCooldown = 1.5f;
    private float lastDamageTime = -Mathf.Infinity;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        pathfinder = FindObjectOfType<AStarGridManager>();

        movement = GetRandomDirection();

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        Debug.Log($"{gameObject.name} initialized. Detection range: {detectionRange}, Move speed: {moveSpeed}");
    }

    protected override void Update()
    {
        if (player == null || pathfinder == null)
        {
            Debug.LogWarning($"{gameObject.name} missing player or pathfinder reference.");
            Wander();
            return;
        }

        if (playerHealth == null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.DrawLine(transform.position, player.transform.position, Color.red);

        if (playerHealth != null && !playerHealth.IsDead && distance < detectionRange)
        {
            Debug.Log($"{gameObject.name} detected player at distance {distance:F2}");

            if (Time.time >= nextRepathTime)
            {
                Debug.Log($"{gameObject.name} recalculating path to player...");
                List<Vector3> newPath = pathfinder.FindPath(transform.position, player.transform.position);
                if (newPath != null && newPath.Count > 0)
                {
                    Debug.Log($"{gameObject.name} found path with {newPath.Count} nodes.");
                    path = newPath;
                    pathIndex = 0;
                }
                else
                {
                    Debug.LogWarning($"{gameObject.name} could not find path to player.");
                }
                nextRepathTime = Time.time + repathInterval;
            }

            if (path != null && pathIndex < path.Count)
            {
                Vector3 target = path[pathIndex];
                Vector2 dir = (target - transform.position).normalized;

                if (Vector2.Distance(transform.position, target) < 0.1f)
                {
                    pathIndex++;
                }

                movement = dir;
            }
            else
            {
                Wander();
            }
        }
        else
        {
            Debug.Log($"{gameObject.name} did not detect player. Distance: {distance:F2}");
            Wander();
        }

        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            movement.y = 0;
        else
            movement.x = 0;

        if (animator != null)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
    }

    protected override void ApplyMovement()
    {
        if (rb != null)
        {
            rb.linearVelocity = movement.normalized * moveSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth != null && Time.time >= lastDamageTime + damageCooldown)
            {
                playerHealth.TakeDamage(damage);
                lastDamageTime = Time.time;
                Debug.Log($"{gameObject.name} hit player. Attempted {damage} damage.");
            }
        }
        else
        {
            Debug.Log($"{gameObject.name} hit a wall or object. Picking new direction.");
            movement = GetRandomDirection();
        }
    }

    protected override void Wander()
    {
        if (Random.Range(0f, 100f) < 10f)
        {
            movement = GetRandomDirection();
            Debug.Log($"{gameObject.name} is wandering in direction {movement}");
        }
    }

    private Vector2 GetRandomDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        return directions[Random.Range(0, directions.Length)];
    }
}






// using UnityEngine;

// public class StraightChaser : BaseCreature
// {
//     private Vector2 lastValidDirection;
//     private float damageCooldown = 1.5f;
//     private float lastDamageTime = -Mathf.Infinity;
//     private Animator animator;

//     // Add a variable to cache the PlayerHealth component
//     private PlayerHealth playerHealth;

//     protected override void Start()
//     {
//         base.Start();
//         animator = GetComponent<Animator>();
//         if (rb == null) rb = GetComponent<Rigidbody2D>();
//         if (rb == null) Debug.LogError("StraightChaser: Rigidbody2D not found!");

//         movement = GetRandomDirection();
//         lastValidDirection = movement;
//         lastDamageTime = -damageCooldown;

//         // Attempt to get the PlayerHealth component at the start if player exists
//         if (player != null)
//         {
//             playerHealth = player.GetComponent<PlayerHealth>();
//             if (playerHealth == null)
//             {
//                 Debug.LogWarning($"StraightChaser: Player GameObject found, but it doesn't have a PlayerHealth component.");
//             }
//         }
//     }

//     protected override void Update()
//     {
//         // --- Modification Starts Here ---

//         // 1. Ensure we have the player and PlayerHealth references
//         // (Could also re-find player if null, depending on game design)
//         if (player == null)
//         {
//             Wander(); // No player in scene, just wander
//             return;
//         }
//         // If we didn't get playerHealth in Start, try again (optional, good if player spawns later)
//         if (playerHealth == null)
//         {
//              playerHealth = player.GetComponent<PlayerHealth>();
//              if (playerHealth == null) {
//                   Wander(); // Player exists but no health component? Wander.
//                   return;
//              }
//         }

//         // 2. Check if the player is dead
//         if (playerHealth.IsDead)
//         {
//             Wander(); // Player is dead, revert to wandering behavior
//         }
//         else // Player exists, has health component, and is NOT dead
//         {
//             // Original chase/wander logic based on distance
//             float distance = Vector2.Distance(transform.position, player.transform.position);
//             if (distance < detectionRange)
//             {
//                 // Chase alive player
//                 movement = (player.transform.position - transform.position).normalized;
//                 lastValidDirection = movement;
//             }
//             else
//             {
//                 // Player is alive but too far away
//                 Wander();
//             }
//         }
//         // --- Modification Ends Here ---

//         // Note: ApplyMovement() call moved to FixedUpdate in the previous step

//         if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
//         {
//             movement.y = 0;
//         }
//         else
//         {
//             movement.x = 0;
//         }

//         if (animator != null)
//         {
//             animator.SetFloat("Horizontal", movement.x);
//             animator.SetFloat("Vertical", movement.y);
//         }
//     }

//     // FixedUpdate remains the same - applies the 'movement' vector calculated in Update
//     protected virtual void FixedUpdate()
//     {
//         ApplyMovement();
//     }


//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Player"))
//         {
//             // No changes needed here. If the player is dead, playerHealth.TakeDamage
//             // will likely do nothing anyway due to its internal checks.
//             // The chaser's own cooldown prevents spamming damage attempts.
//             if (playerHealth != null && Time.time >= lastDamageTime + damageCooldown)
//             {
//                 playerHealth.TakeDamage(damage);
//                 lastDamageTime = Time.time;
//                 Debug.Log($"{gameObject.name} hit player. Attempting {damage} damage.");
//             }
//         }
//         //INSERTED 5/2
//         else
//         {
//             // Try up to 5 new directions that aren't the current or reverse direction
//             for (int i = 0; i < 5; i++)
//             {
//                 Vector2 newDir = GetRandomDirection();
//                 if (newDir != movement && newDir != -movement)
//                 {
//                     movement = newDir;
//                     lastValidDirection = newDir;
//                     break;
//                 }
//             }
//         }

//         // else
//         // {
//         //     movement = GetRandomDirection();
//         //     lastValidDirection = movement;
//         // }
//     }

//     // INSERTED 5/2
//     protected override void Wander()
//     {
//         // Increase chance of changing direction from 1% to 10%
//         if (Random.Range(0f, 100f) < 10f)
//         {
//             Vector2 newDirection = GetRandomDirection();

//             // Ensure it doesn't pick the opposite of its last direction
//             if (newDirection != -lastValidDirection)
//             {
//                 movement = newDirection;
//                 lastValidDirection = newDirection;
//             }
//         }
//         else
//         {
//             movement = lastValidDirection;
//         }
//     }


//     // protected override void Wander()
//     // {
//     //     // Keep wandering logic simple for now
//     //     if (Random.Range(0f, 100f) < 1f)
//     //     {
//     //         movement = GetRandomDirection();
//     //         lastValidDirection = movement;
//     //     }
//     //     else
//     //     {
//     //          movement = lastValidDirection;
//     //     }
//     //     // IMPORTANT: Ensure wander doesn't accidentally use player position
//     //     // This implementation seems safe as it uses random/last direction.
//     // }

//     private Vector2 GetRandomDirection()
//     {
//         Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
//         return directions[Random.Range(0, directions.Length)];
//     }
//     // inserted 5/2
    



//     protected override void ApplyMovement()
//     {
//         if (rb != null)
//         {
            
//              // Check if player is dead AND movement is non-zero (still wandering)
//              if (playerHealth != null && playerHealth.IsDead && movement != Vector2.zero) {
                 
//              }

//              rb.linearVelocity = movement.normalized * moveSpeed;
//         }
//     }
// }