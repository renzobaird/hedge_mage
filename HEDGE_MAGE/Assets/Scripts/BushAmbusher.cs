using UnityEngine;

public class BushAmbusher : BaseCreature
{
    public int hitsToKill = 10;
    private int currentHits = 0;

    protected override void Start()
    {
        base.Start();
        damage = 1; // ✅ Set the inherited damage field from BaseCreature
        gameObject.layer = LayerMask.NameToLayer("EnemyIgnoreBush");
    }

    public override void OnPlayerCollision(GameObject player)
    {
        currentHits++;
        if (currentHits >= hitsToKill)
        {
            Debug.Log("Player killed by Bush Ambusher!");
            // Example logic to damage player
            player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}
