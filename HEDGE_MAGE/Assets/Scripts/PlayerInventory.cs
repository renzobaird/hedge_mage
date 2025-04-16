using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject item;

    public bool HasItem() => item != null;

    public GameObject TakeItem()
    {
        GameObject temp = item;
        item = null;
        return temp;
    }
}
