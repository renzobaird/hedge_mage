using System.Collections.Generic;
using UnityEngine;

public class WordProgressManager : MonoBehaviour
{
    public static WordProgressManager Instance; // 🔥 ADD THIS LINE

    public GameObject letterSlotPrefab;
    public Transform slotContainer;

    [SerializeField] private string[] wordList = { "APPLE", "HOUSE", "LIGHT", "BRICK", "WATER" };
    private string targetWord;
    private HashSet<int> collectedIndexes = new HashSet<int>();
    private List<LetterSlot> letterSlots = new List<LetterSlot>();

    private void Awake()
    {
        // 🔥 Make sure there's only one instance
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
        StartNewGame();
    }

    public void StartNewGame()
    {
        targetWord = GetRandomWord().ToUpper();
        collectedIndexes.Clear();
        SetupUISlots();
    }

    private string GetRandomWord()
    {
        if (wordList.Length == 0) return "PLACE";
        return wordList[Random.Range(0, wordList.Length)];
    }

    private void SetupUISlots()
    {
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

        UpdateSlots();
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
}