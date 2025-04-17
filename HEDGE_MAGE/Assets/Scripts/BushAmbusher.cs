using UnityEngine;

public class BushAmbusher : BaseCreature
{
    public int damage = 1; // Damage specific to the BushAmbusher
    public int hitsToKill = 2;
    private int currentHits = 0;

    protected override void Start()
    {
        base.Start();
        // Can move through bushes (custom collision settings)
        gameObject.layer = LayerMask.NameToLayer("EnemyIgnoreBush");
    }

    public override void OnPlayerCollision(GameObject player)
    {
        currentHits++;
        if (currentHits >= hitsToKill)
        {
            Debug.Log("Player killed by Bush Ambusher!");
            // You'll likely want to add logic here to actually damage the player
            // For example: player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}