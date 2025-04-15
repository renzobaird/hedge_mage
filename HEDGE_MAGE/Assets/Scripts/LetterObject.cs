using UnityEngine;
using TMPro;

public class LetterObject : MonoBehaviour
{
    public char letter;
    public TextMeshPro textMesh;

    public void SetLetter(char c)
    {
        letter = c;
        Debug.Log($"[LetterObject] Setting letter: {letter} on {gameObject.name}");

        if (textMesh != null)
        {
            textMesh.text = c.ToString().ToUpper();
        }
        else
        {
            Debug.LogWarning($"[LetterObject] TextMeshPro not assigned on {gameObject.name}");
        }
    }
}
