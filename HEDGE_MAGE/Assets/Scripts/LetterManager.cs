using UnityEngine;

public class LetterManager
{
    public string[] wordList = {
        "Doomcry",
        "Nighthex",
        "Soulrot",
        "Bloodweb",
        "Darkrune",
        "Gravekin",
        "Fearsong",
        "Voidmaw",
        "Bonecurse",
        "Shadewing",
        "Wraithfire",
        "Cryptborn",
        "Spellbane",
        "Gloommark",
        "Dreamrot",
        "Wyrdseed",
        "Hellbloom",
        "Fleshbind",
        "Starcurse",
        "Ironghast",
        "Abyssmist",
        "Eldritchcry",
        "Voidforge",
        "Necrochant",
        "Blightward"
    };

    string selectedWord = wordList[Random.Range(0, wordList.Length)];

    char[] wordLetters = selectedWord.ToCharArray();



}
