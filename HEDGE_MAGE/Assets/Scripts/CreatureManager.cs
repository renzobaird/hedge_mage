using UnityEngine;
using System.Collections;

public class CreatureManager : MonoBehaviour
{

    public static CreatureManager Instance;

    public GameObject straightChaserPrefab;
    public GameObject flyingThiefPrefab;
    public GameObject bushAmbusherPrefab;

    public float[] spawnDelays = { 10f, 30f, 60f }; // spawn creature 1, then 2, then 3
    private GameObject[] spawnedCreatures = new GameObject[3];

    void Awake()
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
    
    void Start()
    {
        StartCoroutine(SpawnCreatures());
    }

    IEnumerator SpawnCreatures()
    {
        yield return new WaitForSeconds(spawnDelays[0]);
        spawnedCreatures[0] = Instantiate(straightChaserPrefab);

        yield return new WaitForSeconds(spawnDelays[1] - spawnDelays[0]);
        spawnedCreatures[1] = Instantiate(flyingThiefPrefab);

        yield return new WaitForSeconds(spawnDelays[2] - spawnDelays[1]);
        spawnedCreatures[2] = Instantiate(bushAmbusherPrefab);
    }

    // INSERTED CODE 5/1 5PM
    public void ResetCreatures()
    {
        // Deactivate all current creatures
        foreach (var creature in spawnedCreatures)
        {
            if (creature != null)
            {
                creature.SetActive(false); // Deactivate creature
            }
        }

        // Reset spawn delays and start spawning again
        StartCoroutine(SpawnCreatures());
    }

}
