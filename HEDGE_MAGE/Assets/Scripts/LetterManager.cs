using System.Collections.Generic;
using UnityEngine;

public class LetterManager : MonoBehaviour
{
    public static LetterManager Instance;

    public GameObject letterPrefab;
    public List<Transform> spawnPoints;
    public int numberOfDecoys = 5;

    private List<GameObject> spawnedLetters = new List<GameObject>();
    private List<Transform> availableSpots = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SpawnLetters(WordProgressManager.Instance.GetTargetWord());
    }

    public void ResetLettersForNewWord(string newWord)
    {
        // Destroy existing letters
        foreach (var obj in spawnedLetters)
        {
            if (obj != null) Destroy(obj);
        }
        spawnedLetters.Clear();

        // Respawn using new word
        SpawnLetters(newWord);
    }

    private void SpawnLetters(string targetWord)
    {
        availableSpots = new List<Transform>(spawnPoints);
        string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        // Spawn correct letters
        foreach (char c in targetWord)
        {
            SpawnLetter(c);
        }

        // Spawn decoys
        int decoysPlaced = 0;
        while (decoysPlaced < numberOfDecoys && availableSpots.Count > 0)
        {
            char randomLetter = alphabet[Random.Range(0, alphabet.Length)];
            if (!targetWord.Contains(randomLetter.ToString()))
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
        spawnedLetters.Add(obj);

        LetterObject letterObj = obj.GetComponent<LetterObject>();
        if (letterObj != null)
        {
            letterObj.SetLetter(letter);
        }
    }
}



// using System.Collections.Generic;
// using UnityEngine;

// public class LetterManager : MonoBehaviour
// {
//     public static LetterManager Instance;

//     public GameObject letterPrefab;
//     public List<Transform> spawnPoints;
//     public int numberOfDecoys = 5;

//     private List<Transform> availableSpots = new List<Transform>();
//     private List<GameObject> spawnedLetters = new List<GameObject>();

//     void Awake()
//     {
//         if (Instance == null) Instance = this;
//         else Destroy(gameObject);
//     }

//     void Start()
//     {
//         WordProgressManager progress = FindFirstObjectByType<WordProgressManager>();
//         if (progress == null)
//         {
//             Debug.LogError("No WordProgressManager found in scene.");
//             return;
//         }

//         ResetLettersForNewWord(progress.GetTargetWord());
//     }

//     public void ResetLettersForNewWord(string word)
//     {
//         // Destroy old letters
//         foreach (GameObject obj in spawnedLetters)
//         {
//             if (obj != null) Destroy(obj);
//         }
//         spawnedLetters.Clear();

//         // Reset available spots
//         availableSpots = new List<Transform>(spawnPoints);

//         // Spawn correct letters
//         foreach (char c in word)
//         {
//             SpawnLetter(c);
//         }

//         // Spawn decoys
//         string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
//         int decoysPlaced = 0;

//         while (decoysPlaced < numberOfDecoys && availableSpots.Count > 0)
//         {
//             char randomLetter = alphabet[Random.Range(0, alphabet.Length)];

//             if (!word.Contains(randomLetter.ToString()))
//             {
//                 SpawnLetter(randomLetter);
//                 decoysPlaced++;
//             }
//         }
//     }

//     private void SpawnLetter(char letter)
//     {
//         if (availableSpots.Count == 0) return;

//         int index = Random.Range(0, availableSpots.Count);
//         Transform spot = availableSpots[index];
//         availableSpots.RemoveAt(index);

//         GameObject obj = Instantiate(letterPrefab, spot.position, Quaternion.identity);
//         obj.SetActive(true);
//         spawnedLetters.Add(obj);

//         LetterObject letterObj = obj.GetComponent<LetterObject>();
//         if (letterObj != null)
//         {
//             letterObj.SetLetter(letter);
//         }
//     }
// }
