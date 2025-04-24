using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public SpriteRenderer letterRenderer;

    private char letter;

    public void SetLetter(char c, Sprite uncollectedSprite)
    {
        letter = c;
        if (letterRenderer != null)
            letterRenderer.sprite = uncollectedSprite;
        else
            Debug.LogWarning("LetterSlot: letterRenderer is not assigned.");
    }

    public void SetCollectedSprite(Sprite collectedSprite)
    {
        if (letterRenderer != null)
            letterRenderer.sprite = collectedSprite;
        else
            Debug.LogWarning("LetterSlot: letterRenderer is not assigned.");
    }

    public char GetLetter()
    {
        return letter;
    }
}