using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterManager : MonoBehaviour
{
    public GameObject letterPrefab;
    public List<Transform> spawnPoints;
    public int numberOfDecoys = 5;

    private List<Transform> availableSpots = new List<Transform>();

    void Start()
    {
        StartCoroutine(SpawnLettersNextFrame());
    }

    private IEnumerator SpawnLettersNextFrame()
    {
        // Wait 1 frame to ensure WordProgressManager has initialized the new word
        yield return null;

        WordProgressManager progress = FindFirstObjectByType<WordProgressManager>();
        if (progress == null)
        {
            Debug.LogError("No WordProgressManager found in scene.");
            yield break;
        }

        availableSpots = new List<Transform>(spawnPoints);

        foreach (char c in progress.GetTargetWord())
        {
            SpawnLetter(c);
        }

        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int decoysPlaced = 0;

        while (decoysPlaced < numberOfDecoys && availableSpots.Count > 0)
        {
            char randomLetter = alphabet[Random.Range(0, alphabet.Length)];

            if (!progress.GetTargetWord().Contains(randomLetter.ToString()))
            {
                SpawnLetter(randomLetter);
                decoysPlaced++;
            }
        }
    }

    private void SpawnLetter(char letter)
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