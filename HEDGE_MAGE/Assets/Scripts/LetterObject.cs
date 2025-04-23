using UnityEngine;

public class LetterObject : MonoBehaviour
{
    public char letter;
    public SpriteRenderer spriteRenderer;

    public void SetLetter(char c)
    {
        letter = char.ToUpper(c);

        Sprite sprite = LetterSpriteDatabase.Instance.GetSprite(letter, true); // Show collected-style sprite in maze

        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"No sprite found for letter: {letter}");
        }
    }
}