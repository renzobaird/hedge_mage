using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordProgressManager : MonoBehaviour
{
    public string targetWord;
    private HashSet<int> collectedIndexes = new HashSet<int>();

    public TextMeshProUGUI wordDisplayText;

    private void Start()
    {
        UpdateDisplay();
    }

    public void SetWord(string word)
    {
        targetWord = word.ToUpper();
        collectedIndexes.Clear();
        UpdateDisplay();
    }

    public void CollectLetter(char collected)
    {
        collected = char.ToUpper(collected);

        for (int i = 0; i < targetWord.Length; i++)
        {
            if (targetWord[i] == collected && !collectedIndexes.Contains(i))
            {
                collectedIndexes.Add(i);
                break;
            }
        }

        UpdateDisplay();

        if (collectedIndexes.Count == targetWord.Length)
        {
            Debug.Log("All letters collected! Level complete!");
            // Insert your level complete logic here
        }
    }

    private void UpdateDisplay()
    {
        string result = "";

        for (int i = 0; i < targetWord.Length; i++)
        {
            if (collectedIndexes.Contains(i))
                result += $"<color=green>{targetWord[i]}</color>";
            else
                result += $"<color=white>{targetWord[i]}</color>";
        }

        if (wordDisplayText != null)
            wordDisplayText.text = result;
    }
}