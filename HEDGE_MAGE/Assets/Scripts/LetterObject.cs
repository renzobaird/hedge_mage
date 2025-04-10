using UnityEngine;
using TMPro;

public class LetterObject : MonoBehaviour
{
    public char letter;
    public TextMeshPro textMesh;

    public void SetLetter(char c)
    {
        letter = c;
        if (textMesh != null)
        {
            textMesh.text = c.ToString().ToUpper();
        }
    }
}
