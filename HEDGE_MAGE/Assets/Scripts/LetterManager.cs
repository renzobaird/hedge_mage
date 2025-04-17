using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LetterManager : MonoBehaviour
{
    public string[] wordList = { "apple", "train", "cloud", "brick", "mouse" };
    public GameObject letterPrefab;
    public List<Transform> spawnPoints;
    private List<Transform> availableSpots;
    public int numberOfDecoys = 5;
    public List<char> letters;  // Holds all correct letters, useful if needed elsewhere

    public TextMeshProUGUI wordDisplayText; // UI Text to display the selected word

    private string selectedWord;

    void Start()
    {
        // Choose a random word and display it
        selectedWord = wordList[Random.Range(0, wordList.Length)];
        DisplaySelectedWord();

        selectedWord = wordList[Random.Range(0, wordList.Length)];
        DisplaySelectedWord();

        // Tell the WordProgressManager
        FindFirstObjectByType<WordProgressManager>().SetWord(selectedWord);

        // Store letters for use by other scripts if needed
        letters = new List<char>(selectedWord.ToCharArray());

        // Clone the spawn points so we can safely remove from the list
        availableSpots = new List<Transform>(spawnPoints);

        // Spawn correct letters
        foreach (char letter in letters)
        {
            if (availableSpots.Count == 0) break;

            int index = Random.Range(0, availableSpots.Count);
            Transform spot = availableSpots[index];
            availableSpots.RemoveAt(index);

            GameObject obj = Instantiate(letterPrefab, spot.position, Quaternion.identity);
            obj.SetActive(true); // just in case it's disabled
            obj.GetComponent<LetterObject>().SetLetter(letter);
        }

        // Spawn decoy letters
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        for (int i = 0; i < numberOfDecoys && availableSpots.Count > 0; i++)
        {
            int index = Random.Range(0, availableSpots.Count);
            Transform spot = availableSpots[index];
            availableSpots.RemoveAt(index);

            char randomLetter = alphabet[Random.Range(0, alphabet.Length)];
            GameObject obj = Instantiate(letterPrefab, spot.position, Quaternion.identity);
            obj.SetActive(true); // just in case
            obj.GetComponent<LetterObject>().SetLetter(randomLetter);
        }
    }

    void DisplaySelectedWord()
    {
        Debug.Log("Selected Word: " + selectedWord);

        if (wordDisplayText != null)
        {
            wordDisplayText.text = " " + selectedWord.ToUpper();
        }
        else
        {
            Debug.LogWarning("Word Display Text is not assigned in the inspector.");
        }
    }
}