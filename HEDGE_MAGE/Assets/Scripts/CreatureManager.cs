// using UnityEngine;
// using System.Collections;

// public class CreatureManager : MonoBehaviour
// {

//     public static CreatureManager Instance;

//     public GameObject straightChaserPrefab;
//     public GameObject flyingThiefPrefab;
//     public GameObject bushAmbusherPrefab;

//     public float[] spawnDelays = { 10f, 30f, 60f }; // spawn creature 1, then 2, then 3
//     private GameObject[] spawnedCreatures = new GameObject[3];

//     void Awake()
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
    
//     void Start()
//     {
//         StartCoroutine(SpawnCreatures());
//     }

//     IEnumerator SpawnCreatures()
//     {
//         yield return new WaitForSeconds(spawnDelays[0]);
//         spawnedCreatures[0] = Instantiate(straightChaserPrefab);

//         yield return new WaitForSeconds(spawnDelays[1] - spawnDelays[0]);
//         spawnedCreatures[1] = Instantiate(flyingThiefPrefab);

//         yield return new WaitForSeconds(spawnDelays[2] - spawnDelays[1]);
//         spawnedCreatures[2] = Instantiate(bushAmbusherPrefab);
//     }

//     // INSERTED CODE 5/1 5PM
//     public void ResetCreatures()
//     {
//         // Destroy all existing creatures
//         for (int i = 0; i < spawnedCreatures.Length; i++)
//         {
//             if (spawnedCreatures[i] != null)
//             {
//                 Destroy(spawnedCreatures[i]);
//             }
//         }

//         // Clear array and restart spawning
//         spawnedCreatures = new GameObject[3];
//         StopAllCoroutines(); // Ensure no overlapping spawns
//         StartCoroutine(SpawnCreatures());
//     }

// }


using UnityEngine;
using System.Collections;

public class CreatureManager : MonoBehaviour
{
    public static CreatureManager Instance;

    [Header("StraightChaser Setup")]
    public GameObject straightChaserPrefab;
    public Transform[] straightChaserSpawnPoints; // Assign 3 spawn points here

    private GameObject[] spawnedChasers;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SpawnStraightChasers();
    }

    public void SpawnStraightChasers()
    {
        if (straightChaserPrefab == null || straightChaserSpawnPoints.Length == 0) return;

        spawnedChasers = new GameObject[straightChaserSpawnPoints.Length];

        for (int i = 0; i < straightChaserSpawnPoints.Length; i++)
        {
            if (straightChaserSpawnPoints[i] == null) continue;

            GameObject chaser = Instantiate(straightChaserPrefab, straightChaserSpawnPoints[i].position, Quaternion.identity);
            spawnedChasers[i] = chaser;
        }
    }

    public void ResetCreatures()
    {
        if (spawnedChasers != null)
        {
            foreach (var creature in spawnedChasers)
            {
                if (creature != null)
                    Destroy(creature);
            }
        }

        SpawnStraightChasers();
    }
}
