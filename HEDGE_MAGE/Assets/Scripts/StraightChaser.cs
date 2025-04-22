using UnityEngine;
using System.Collections.Generic;

public class StraightChaser : BaseCreature
{
    [Header("StraightChaser Pathfinding")]
    public float repathRate = 0.7f;
    public float nodeReachThreshold = 0.2f;

    private float lastPathTime;
    private Queue<Vector3> currentPath = new Queue<Vector3>();

    private AStarGridManager pathfinder;

    protected override void Start()
    {
        base.Start();
        damage = 25;
        pathfinder = FindFirstObjectByType<AStarGridManager>();

        if (pathfinder == null)
        {
            Debug.LogError("No AStarGridManager found in scene!");
        }

        movement = Vector2.right; // Default start direction for wandering
    }

    protected override void Update()
    {
        if (player == null || pathfinder == null)
        {
            WanderWithMemory();
            ApplyMovement();
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < detectionRange)
        {
            if (Time.time - lastPathTime > repathRate)
            {
                lastPathTime = Time.time;
                List<Vector3> path = pathfinder.FindPath(transform.position, player.transform.position);
                if (path != null && path.Count > 0)
                {
                    currentPath = new Queue<Vector3>(path);
                }
            }

            FollowPath();
        }
        else
        {
            WanderWithMemory();
        }

        ApplyMovement();
    }

    private void FollowPath()
    {
        if (currentPath.Count == 0)
        {
            movement = Vector2.zero;
            return;
        }

        Vector3 target = currentPath.Peek();
        Vector2 direction = (target - transform.position).normalized;

        if (Vector2.Distance(transform.position, target) < nodeReachThreshold)
        {
            currentPath.Dequeue();
        }

        movement = direction;
    }

    private void WanderWithMemory()
    {
        if (Random.Range(0f, 100f) < 2f)
        {
            float angle = Random.Range(-90f, 90f);
            movement = Quaternion.Euler(0, 0, angle) * movement;
        }

        movement = movement.normalized;
    }

    private void ApplyMovement()
    {
        if (rb != null)
        {
            rb.linearVelocity = movement * moveSpeed;
        }
    }

    public override void OnPlayerCollision(GameObject player)
    {
        Debug.Log("Straight Chaser specific OnPlayerCollision code executed.");
    }
}