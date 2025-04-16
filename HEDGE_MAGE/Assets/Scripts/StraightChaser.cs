using UnityEngine;

public class StraightChaser : BaseCreature
{
    public float speedMultiplier = 1.5f;
    public int damage = 1;
    public int hitsToKill = 3;
    private int currentHits = 0;

    protected override void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < detectionRange)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            if (Mathf.Abs(direction.x) < 0.1f || Mathf.Abs(direction.y) < 0.1f)
                rb.linearVelocity = direction * moveSpeed * speedMultiplier;
            else
                rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            base.Update();
        }
    }

    public override void OnPlayerCollision(GameObject player)
    {
        currentHits++;
        if (currentHits >= hitsToKill)
        {
            Debug.Log("Player killed by Straight Chaser!");
        }
    }
}
