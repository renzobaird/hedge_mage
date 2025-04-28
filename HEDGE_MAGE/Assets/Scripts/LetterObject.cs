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
            spriteRenderer.sprite = LetterSpriteDatabase.Instance.GetUncollectedSprite(letter);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WordProgressManager progress = FindFirstObjectByType<WordProgressManager>();
            if (progress != null)
            {
                progress.CollectLetter(letter);
                gameObject.SetActive(false);
            }
        }
    }
}