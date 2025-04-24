using UnityEngine;
using System.Collections.Generic;

public class LetterManager : MonoBehaviour
{
    public string[] wordList = { "apple", "train", "cloud", "brick", "mouse" };
    public GameObject letterPrefab; // Maze letter prefab (has SpriteRenderer)
    public List<Transform> spawnPoints;
    public int numberOfDecoys = 5;

    private List<Transform> availableSpots;
    private string selectedWord;

    void Start()
    {
        selectedWord = wordList[Random.Range(0, wordList.Length)];

        // Set word in WordProgressManager — it now generates UI on its own
        WordProgressManager progress = FindFirstObjectByType<WordProgressManager>();
        if (progress != null)
        {
            progress.SetWord(selectedWord);
        }

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
}