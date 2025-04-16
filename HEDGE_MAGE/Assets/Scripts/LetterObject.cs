using UnityEngine;
using TMPro;

public class LetterObject : MonoBehaviour
{
    public char letter;
    public TextMeshProUGUI textMesh;

    public void SetLetter(char c)
    {
        letter = c;

        if (textMesh != null)
        {
            textMesh.text = c.ToString().ToUpper();
            Debug.Log($"LetterObject: Set letter to {letter}");
        }
        else
        {
            Debug.LogWarning("LetterObject: TextMeshProUGUI reference is missing!");
        }
    }
}