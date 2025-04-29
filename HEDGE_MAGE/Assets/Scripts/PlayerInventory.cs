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

            // Report to WordProgressManager
            WordProgressManager progress = FindFirstObjectByType<WordProgressManager>();
            if (progress != null)
            {
                progress.CollectLetter(collectedChar);
            }

            // Store if you want, or ignore — this is optional
            item = other.gameObject;

            //  Destroy the letter object, whether correct or decoy
            Destroy(other.gameObject);

            Debug.Log($"Collected letter: {collectedChar}");
        }
    }
}