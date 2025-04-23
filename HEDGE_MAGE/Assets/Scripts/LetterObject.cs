using UnityEngine;

public class LetterObject : MonoBehaviour
{
    public char letter;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Auto-assign if not set manually
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetLetter(char c)
    {
        letter = char.ToUpper(c);

        if (spriteRenderer != null)
        {
            Sprite letterSprite = LetterSpriteDatabase.Instance.GetSpriteForLetter(letter);
            if (letterSprite != null)
            {
                spriteRenderer.sprite = letterSprite;
                gameObject.name = $"Letter_{letter}"; // Helpful for debugging in Hierarchy
            }
            else
            {
                Debug.LogWarning($"No sprite found for letter: {letter}");
            }
        }
        else
        {
            Debug.LogWarning("SpriteRenderer reference is missing!");
        }
    }
}