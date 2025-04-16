using UnityEngine;

public class FlyingThief : BaseCreature
{
    public float dropDistance = 5f;
    private GameObject stolenItem;

    protected override void Update()
    {
        base.Update();
    }

    public override void OnPlayerCollision(GameObject player)
    {
        if (stolenItem == null)
        {
            var inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null && inventory.HasItem())
            {
                stolenItem = inventory.TakeItem();
                Vector2 dropLocation = (Vector2)player.transform.position + Random.insideUnitCircle * dropDistance;
                stolenItem.transform.position = dropLocation;
                stolenItem.SetActive(true);
                stolenItem = null;
            }
        }
    }
}
