using UnityEngine;

public class LetterObject : MonoBehaviour
{
    public char letter;
    public SpriteRenderer spriteRenderer;

    public void SetLetter(char c)
    {
        letter = char.ToUpper(c);

        bool collected = WordProgressManager.Instance != null && WordProgressManager.Instance.IsLetterCollected(letter);
        Sprite sprite = LetterSpriteDatabase.Instance.GetSprite(letter, collected);

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogError($"{gameObject.name} is missing a spriteRenderer reference!");
        }
    }

    public void OnCollect()
    {
        gameObject.SetActive(false);
        WordProgressManager.Instance?.CollectLetter(letter);
    }

    
}