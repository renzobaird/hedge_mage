using UnityEngine;
using System.Collections.Generic;

public class LetterManager : MonoBehaviour
{
    public string[] wordList = { "apple", "train", "cloud", "brick", "mouse" };
    public GameObject letterPrefab; // Maze letter prefab (has SpriteRenderer)
    public List<Transform> spawnPoints;
    public int numberOfDecoys = 5;

    public GameObject wordDisplayParent; // UI container with HorizontalLayoutGroup
    public GameObject letterSlotPrefab;  // UI letter slot prefab

    private List<Transform> availableSpots;
    private string selectedWord;
    private List<GameObject> uiLetterSlots = new List<GameObject>();
    
    
    void Start()
    {
        selectedWord = wordList[Random.Range(0, wordList.Length)];

        // // Set word in ProgressManager
        // WordProgressManager progress = FindFirstObjectByType<WordProgressManager>();
        // if (progress != null)
        // {
        //     progress.SetWord(selectedWord);
        //     progress.SetUISlots(uiLetterSlots); // Pass slots for UI updating
        // }
    private void GenerateUISlots(string word);
    {
        List<LetterSlot> uiSlots = new List<LetterSlot>();

        foreach (char letter in word.ToUpper())
        {
            GameObject slotGO = Instantiate(letterSlotPrefab, uiLetterSlotParent);
            LetterSlot slot = slotGO.GetComponent<LetterSlot>();

            if (slot != null)
            {
                uiSlots.Add(slot); // ✅ Only add the actual LetterSlot, not GameObject
            }
            else
            {
                Debug.LogWarning("Missing LetterSlot component on prefab!");
            }
        }

        WordProgressManager progress = FindFirstObjectByType<WordProgressManager>();
        progress.SetUISlots(uiSlots); // ✅ Now you're passing List<LetterSlot>
    }

        // Create UI letter display
        CreateUILetterDisplay(selectedWord);

        // Spawn letter objects into maze
        availableSpots = new List<Transform>(spawnPoints);

        // Spawn correct letters
        foreach (char letter in selectedWord.ToUpper())
        {
            SpawnLetter(letter);
        }

        // Spawn decoy letters
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int decoysPlaced = 0;

        while (decoysPlaced < numberOfDecoys && availableSpots.Count > 0)
        {
            char randomLetter = alphabet[Random.Range(0, alphabet.Length)];

            // Prevent duplicates with main word
            if (!selectedWord.ToUpper().Contains(randomLetter.ToString()))
            {
                SpawnLetter(randomLetter);
                decoysPlaced++;
            }
        }
    }

    void SpawnLetter(char letter)
    {
        if (availableSpots.Count == 0) return;

        int index = Random.Range(0, availableSpots.Count);
        Transform spot = availableSpots[index];
        availableSpots.RemoveAt(index);

        GameObject obj = Instantiate(letterPrefab, spot.position, Quaternion.identity);
        obj.SetActive(true);

        LetterObject letterObj = obj.GetComponent<LetterObject>();
        if (letterObj != null)
        {
            letterObj.SetLetter(letter);
        }
    }

    void CreateUILetterDisplay(string word)
    {
        foreach (char c in word.ToUpper())
        {
            GameObject slot = Instantiate(letterSlotPrefab, wordDisplayParent.transform);
            uiLetterSlots.Add(slot);
        }
    }
}