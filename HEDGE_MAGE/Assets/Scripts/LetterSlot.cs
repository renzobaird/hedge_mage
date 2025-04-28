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
    }

    public void SetCollectedSprite(Sprite collectedSprite)
    {
        if (letterRenderer != null)
            letterRenderer.sprite = collectedSprite;
    }

    public char GetLetter()
    {
        return letter;
    }
}