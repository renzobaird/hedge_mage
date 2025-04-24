using System.Collections.Generic;
using UnityEngine;

public class WordProgressManager : MonoBehaviour
{
    public static WordProgressManager Instance;

    [Header("UI Components")]
    public GameObject letterSlotPrefab;
    public Transform slotContainer;

    [Header("Word Settings")]
    [SerializeField] private string[] wordList = { "APPLE", "HOUSE", "LIGHT", "BRICK", "WATER" };

    public string targetWord { get; private set; }

    private HashSet<int> collectedIndexes = new HashSet<int>();
    private List<LetterSlot> letterSlots = new List<LetterSlot>();
    private bool gameStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Optional: Auto-start a game when this script first loads
        // StartNewGame();
    }

    public void StartNewGame()
    {
        if (gameStarted) return;

        targetWord = GetRandomWord().ToUpper();
        collectedIndexes.Clear();
        SetupUISlots();
        UpdateUIDisplay();
        gameStarted = true;
    }

    public void ResetProgress()
    {
        collectedIndexes.Clear();
        UpdateUIDisplay();
    }

    private string GetRandomWord()
    {
        if (wordList == null || wordList.Length == 0) return "PLACE";
        int index = Random.Range(0, wordList.Length);
        return wordList[index];
    }

    private void SetupUISlots()
    {
        if (slotContainer == null) return;

        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        letterSlots.Clear();

        foreach (char c in targetWord)
        {
            GameObject slotObj = Instantiate(letterSlotPrefab, slotContainer);
            LetterSlot slot = slotObj.GetComponent<LetterSlot>();

            Sprite uncollectedSprite = LetterSpriteDatabase.Instance.GetUncollectedSprite(c);
            slot.SetLetter(c, uncollectedSprite);

            letterSlots.Add(slot);
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

        UpdateUIDisplay();
    }

    private void UpdateUIDisplay()
    {
        if (letterSlots == null || letterSlots.Count == 0) return;

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
            {
                return true;
            }
        }
        return false;
    }

    // Call this when starting a new session (like from a Play button in title screen)
    public void ForceRestart()
    {
        gameStarted = false;
        StartNewGame();
    }
}