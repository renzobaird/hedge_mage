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

    private void OnTriggerEnter2D(Collider2D other)
    {
        LetterObject letterObj = other.GetComponent<LetterObject>();

        if (letterObj != null)
        {
            char collectedChar = letterObj.letter;

            // Report to progress tracker
            WordProgressManager progress = FindFirstObjectByType<WordProgressManager>();
            if (progress != null)
            {
                progress.CollectLetter(collectedChar);
            }

            // Optionally store the letter in inventory
            item = other.gameObject;

            // Destroy or disable the letter
            Destroy(other.gameObject);

            Debug.Log($"Collected letter: {collectedChar}");
        }
    }
}