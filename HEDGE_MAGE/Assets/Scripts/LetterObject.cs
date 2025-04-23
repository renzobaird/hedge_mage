using UnityEngine;

public class LetterObject : MonoBehaviour
{
    public char letter;
    public SpriteRenderer spriteRenderer;

    public void SetLetter(char c)
    {
        letter = char.ToUpper(c);

        Sprite sprite = LetterSpriteDatabase.Instance.GetSpriteForLetter(letter);

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