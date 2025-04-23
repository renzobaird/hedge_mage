using System.Collections.Generic;
using UnityEngine;

public class WordProgressManager : MonoBehaviour
{
    public string targetWord;
    private HashSet<int> collectedIndexes = new HashSet<int>();
    private List<LetterSlot> letterSlots = new List<LetterSlot>();

    public void SetWord(string word)
    {
        targetWord = word.ToUpper();
        collectedIndexes.Clear();
        // Do NOT call UpdateUIDisplay yet — wait until UI slots are assigned
    }

    public void SetUISlots(List<LetterSlot> slots)
    {
        letterSlots = slots;

        // Make sure number of slots matches target word
        if (letterSlots.Count != targetWord.Length)
        {
            Debug.LogWarning("Mismatch between slot count and word length!");
        }

        UpdateUIDisplay();
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

        UpdateUIDisplay();

        if (collectedIndexes.Count == targetWord.Length)
        {
            Debug.Log("All letters collected! Level complete!");
            // Trigger level complete logic here
        }
    }

    private void UpdateUIDisplay()
    {
        for (int i = 0; i < targetWord.Length; i++)
        {
            if (i < letterSlots.Count)
            {
                if (collectedIndexes.Contains(i))
                {
                    letterSlots[i].SetLetter(targetWord[i], true); // collected = true
                }
                else
                {
                    letterSlots[i].SetLetter(targetWord[i], false); // collected = false
                }
            }
        }
    }
}