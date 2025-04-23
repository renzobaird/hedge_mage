using UnityEngine;
using UnityEngine.UI;

public class LetterSlot : MonoBehaviour
{
    public Image letterImage;
    public Sprite filledSprite;
    public Sprite emptySprite;

    public void SetLetter(char letter, bool collected)
    {
        if (letterImage != null)
        {
            letterImage.sprite = collected ? filledSprite : emptySprite;
        }
    }
}