using UnityEngine;

public class LetterObject : MonoBehaviour
{
    public char letter;
    public SpriteRenderer spriteRenderer;

    public void SetLetter(char c)
    {
        letter = char.ToUpper(c);

        if (spriteRenderer != null)
        {
            bool alreadyCollected = WordProgressManager.Instance != null && WordProgressManager.Instance.IsLetterCollected(letter);

            spriteRenderer.sprite = alreadyCollected
                ? LetterSpriteDatabase.Instance.GetCollectedSprite(letter)
                : LetterSpriteDatabase.Instance.GetUncollectedSprite(letter);
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

            gameObject.SetActive(false);
        }
    }
}