using System.Collections.Generic;
using UnityEngine;

public class WordProgressManager : MonoBehaviour
{
    public static WordProgressManager Instance;

    public GameObject letterSlotPrefab;
    public Transform slotContainer;

    [SerializeField] private string[] wordList = { "APPLE", "HOUSE", "LIGHT", "BRICK", "WATER" };
    private string targetWord;
    private int currentWordIndex = 0;
    private HashSet<int> collectedIndexes = new HashSet<int>();
    private List<LetterSlotUI> letterSlots = new List<LetterSlotUI>();

    public GameObject bookPopup;
    public GameObject levelCompletePopup;
    public GameObject levelFailPopup;

    public Transform levelStartSpawnPoint;

    private bool isRetrying = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartNewGame();
    }

    public void RetrySameWord()
    {
        isRetrying = true;
        RestartLevel();
    }

    public void AdvanceToNextWord()
    {
        isRetrying = false;
        RestartLevel();
    }

    private string GetNextWord()
    {
        if (wordList.Length == 0) return "PLACE";

        if (isRetrying)
        {
            isRetrying = false;
            return wordList[(currentWordIndex - 1 + wordList.Length) % wordList.Length];
        }
        else
        {
            string word = wordList[currentWordIndex % wordList.Length];
            currentWordIndex++;
            return word;
        }
    }

    public void StartNewGame()
    {
        targetWord = GetNextWord().ToUpper();
        collectedIndexes.Clear();
        SetupUISlots();
    }

    private void SetupUISlots()
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        letterSlots.Clear();

        foreach (char c in targetWord)
        {
            GameObject slotObj = Instantiate(letterSlotPrefab, slotContainer);
            LetterSlotUI slot = slotObj.GetComponent<LetterSlotUI>();
            if (slot != null)
            {
                Sprite uncollected = LetterSpriteDatabase.Instance.GetUncollectedSprite(c);
                slot.SetLetter(c, uncollected);
                letterSlots.Add(slot);
            }
        }
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

        UpdateSlots();

        if (IsWordComplete())
        {
            LevelPopupManager.Instance.ShowLevelCompletePopup(Time.timeSinceLevelLoad);
        }
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < targetWord.Length; i++)
        {
            if (i < letterSlots.Count)
            {
                char letter = targetWord[i];
                Sprite sprite = collectedIndexes.Contains(i)
                    ? LetterSpriteDatabase.Instance.GetCollectedSprite(letter)
                    : LetterSpriteDatabase.Instance.GetUncollectedSprite(letter);

                letterSlots[i].SetCollectedSprite(sprite);
            }
        }
    }

    public bool IsLetterCollected(char c)
    {
        c = char.ToUpper(c);
        for (int i = 0; i < targetWord.Length; i++)
        {
            if (targetWord[i] == c && collectedIndexes.Contains(i))
                return true;
        }
        return false;
    }

    public string GetTargetWord()
    {
        return targetWord;
    }

    private bool IsWordComplete()
    {
        return collectedIndexes.Count == targetWord.Length;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        // Refresh the word
        targetWord = GetNextWord().ToUpper();
        collectedIndexes.Clear();
        SetupUISlots();

        // Reset spawned letters
        if (LetterManager.Instance != null)
            LetterManager.Instance.ResetLettersForNewWord(targetWord);

        // Reset player
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.ResetForNewLevel();
            PlayerHealth.Instance.MoveToLevelStart(levelStartSpawnPoint);
        }

        // Hide popups
        if (levelCompletePopup != null) levelCompletePopup.SetActive(false);
        if (levelFailPopup != null) levelFailPopup.SetActive(false);
        if (bookPopup != null) bookPopup.SetActive(false);
    }
}




// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class WordProgressManager : MonoBehaviour
// {
//     public static WordProgressManager Instance;

//     public GameObject letterSlotPrefab;
//     public Transform slotContainer;

//     [SerializeField] private string[] wordList = { "APPLE", "HOUSE", "LIGHT", "BRICK", "WATER" };
//     private string targetWord;
//     private int currentWordIndex = 0;
//     private HashSet<int> collectedIndexes = new HashSet<int>();
//     private List<LetterSlotUI> letterSlots = new List<LetterSlotUI>();

//     public GameObject bookPopup;
//     public GameObject levelCompletePopup;
//     public GameObject levelFailPopup;

//     public Transform levelStartSpawnPoint;

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void Start()
//     {
//         StartNewGame();
//     }

//     public void StartNewGame()
//     {
//         targetWord = GetNextWord().ToUpper();
//         collectedIndexes.Clear();
//         SetupUISlots();
//     }

//     // NEW CODE INSERTED HERE
//     private bool isRetrying = false;

//     public void RetrySameWord()
//     {
//         isRetrying = true;
//         RestartLevel();
//     }

//     public void AdvanceToNextWord()
//     {
//         isRetrying = false;
//         RestartLevel();
//     }
//     // END OF INSTERTED CODE

//     private string GetNextWord()
//     {
//         if (wordList.Length == 0) return "PLACE";

//         if (isRetrying)
//         {
//             // Reset retry flag and return the same word
//             isRetrying = false;
//             return wordList[(currentWordIndex - 1 + wordList.Length) % wordList.Length];
//         }
//         else
//         {
//             string word = wordList[currentWordIndex % wordList.Length];
//             currentWordIndex++;
//             return word;
//         }
//     }

//     // ORIGINAL CODE FOR GETNEXTWORD()
//     // if (wordList.Length == 0) return "PLACE";
//     //     string word = wordList[currentWordIndex % wordList.Length];
//     //     currentWordIndex++;
//     //     return word;
//     // }

//     private void SetupUISlots()
//     {
//         Debug.Log($"Clearing previous letter slots. Existing count: {slotContainer.childCount}");

//         foreach (Transform child in slotContainer)
//         {
//             Destroy(child.gameObject);
//         }
//         letterSlots.Clear();

//         Debug.Log($"Setting up slots for new word: {targetWord}");

//         foreach (char c in targetWord)
//         {
//             GameObject slotObj = Instantiate(letterSlotPrefab, slotContainer);
//             LetterSlotUI slot = slotObj.GetComponent<LetterSlotUI>();

//             if (slot == null)
//             {
//                 Debug.LogError("Instantiated LetterSlot prefab is missing the LetterSlot component.");
//                 continue;
//             }

//             Sprite uncollectedSprite = LetterSpriteDatabase.Instance.GetUncollectedSprite(c);
//             if (uncollectedSprite == null)
//             {
//                 Debug.LogWarning($"Missing uncollected sprite for letter: {c}");
//             }

//             slot.SetLetter(c, uncollectedSprite);
//             letterSlots.Add(slot);

//             Debug.Log($"Slot created for letter: {c}");
//         }

//         Debug.Log($"Total slots created: {letterSlots.Count}");
//     }

//     public void CollectLetter(char collected)
//     {
//         collected = char.ToUpper(collected);

//         for (int i = 0; i < targetWord.Length; i++)
//         {
//             if (targetWord[i] == collected && !collectedIndexes.Contains(i))
//             {
//                 collectedIndexes.Add(i);
//                 break;
//             }
//         }

//         UpdateSlots();

//         if (IsWordComplete())
//         {
//             ShowLevelCompletePopup();
//         }
//     }

//     private void UpdateSlots()
//     {
//         for (int i = 0; i < targetWord.Length; i++)
//         {
//             if (i < letterSlots.Count)
//             {
//                 char letter = targetWord[i];
//                 Sprite sprite = collectedIndexes.Contains(i)
//                     ? LetterSpriteDatabase.Instance.GetCollectedSprite(letter)
//                     : LetterSpriteDatabase.Instance.GetUncollectedSprite(letter);

//                 letterSlots[i].SetCollectedSprite(sprite);
//             }
//         }
//     }

//     public bool IsLetterCollected(char c)
//     {
//         c = char.ToUpper(c);
//         for (int i = 0; i < targetWord.Length; i++)
//         {
//             if (targetWord[i] == c && collectedIndexes.Contains(i))
//                 return true;
//         }
//         return false;
//     }

//     public string GetTargetWord()
//     {
//         return targetWord;
//     }

//     private bool IsWordComplete()
//     {
//         return collectedIndexes.Count == targetWord.Length;
//     }

//     private void ShowLevelCompletePopup()
//     {
//         if (levelCompletePopup != null)
//         {
//             levelCompletePopup.SetActive(true);
//             Time.timeScale = 0f;
//         }
//     }

//     public void ShowLevelFailPopup()
//     {
//         if (levelFailPopup != null)
//         {
//             levelFailPopup.SetActive(true);
//             Time.timeScale = 0f;
//         }
//     }

//     public void RestartLevel()
//     {
//         Time.timeScale = 1f; // <-- Ensure unpausing always
//         // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        
//         // Reset word and UI
//         targetWord = GetNextWord().ToUpper();
//         collectedIndexes.Clear();
//         SetupUISlots();

//         // // Reset spawned letters
//         LetterManager.Instance.ResetLettersForNewWord(targetWord);

//         // // Optionally reset player health/position if needed
//         PlayerHealth.Instance.ResetForNewLevel();
//         PlayerHealth.Instance.MoveToLevelStart(levelStartSpawnPoint);


//         // Hide popups
//         if (levelCompletePopup != null) levelCompletePopup.SetActive(false);
//         if (levelFailPopup != null) levelFailPopup.SetActive(false);
//         if (bookPopup != null) bookPopup.SetActive(false);
//         }

// }