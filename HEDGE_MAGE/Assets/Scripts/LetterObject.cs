using UnityEngine;

public class LetterObject : MonoBehaviour
{
    
    public char letter;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("LetterObject: SpriteRenderer is missing on the GameObject.");
            }
        }
    }


    public void SetLetter(char c)
    {
        letter = char.ToUpper(c);

        if (spriteRenderer != null)
        {
            // Use world sprite only (new third set)
            spriteRenderer.sprite = LetterSpriteDatabase.Instance.GetWorldSprite(letter);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (WordProgressManager.Instance != null)
            {
                WordProgressManager.Instance.CollectLetter(letter);
            }

            Debug.Log($"Letter '{letter}' collected by player at position {transform.position}.");

            // Hide letter instead of destroying it, if reuse is possible
            gameObject.SetActive(false);
        }
    }
}
