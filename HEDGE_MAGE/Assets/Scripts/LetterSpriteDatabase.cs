using System.Collections.Generic;
using UnityEngine;

public class LetterSpriteDatabase : MonoBehaviour
{
    public static LetterSpriteDatabase Instance;

    [System.Serializable]
    public struct LetterSpritePair
    {
        public char letter;
        public Sprite sprite;
    }

    [Header("Set these in the Inspector")]
    public List<LetterSpritePair> letterSprites;

    private Dictionary<char, Sprite> letterSpriteDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            BuildDictionary();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void BuildDictionary()
    {
        letterSpriteDict = new Dictionary<char, Sprite>();
        foreach (var pair in letterSprites)
        {
            char upper = char.ToUpper(pair.letter);
            if (!letterSpriteDict.ContainsKey(upper))
            {
                letterSpriteDict.Add(upper, pair.sprite);
            }
            else
            {
                Debug.LogWarning($"Duplicate letter sprite entry for: {upper}");
            }
        }
    }

    public Sprite GetSpriteForLetter(char c)
    {
        c = char.ToUpper(c);
        if (letterSpriteDict.TryGetValue(c, out Sprite sprite))
        {
            return sprite;
        }

        Debug.LogWarning($"No sprite found for letter: {c}");
        return null;
    }
}