using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LetterManager : MonoBehaviour
{
    public string[] wordList = { "apple", "train", "cloud", "brick", "mouse" };
    public GameObject letterPrefab;
    public List<Transform> spawnPoints;
    public int numberOfDecoys = 5;

    private string selectedWord;

    void Start()
    {
        selectedWord = wordList[Random.Range(0, wordList.Length)];
        char[] letters = selectedWord.ToCharArray();

        List<Transform> availableSpots = new List<Transform>(spawnPoints);

        // Spawn correct letters
        foreach (char letter in letters)
        {
            if (availableSpots.Count == 0) break;

            int index = Random.Range(0, availableSpots.Count);
            Transform spot = availableSpots[index];
            availableSpots.RemoveAt(index);

            GameObject obj = Instantiate(letterPrefab, spot.position, Quaternion.identity);
            obj.GetComponent<LetterObject>().SetLetter(letter);
        }

        // Spawn decoys
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        for (int i = 0; i < numberOfDecoys && availableSpots.Count > 0; i++)
        {
            int index = Random.Range(0, availableSpots.Count);
            Transform spot = availableSpots[index];
            availableSpots.RemoveAt(index);

            char randomLetter = alphabet[Random.Range(0, alphabet.Length)];
            GameObject obj = Instantiate(letterPrefab, spot.position, Quaternion.identity);
            obj.GetComponent<LetterObject>().SetLetter(randomLetter);
        }
    }
}
