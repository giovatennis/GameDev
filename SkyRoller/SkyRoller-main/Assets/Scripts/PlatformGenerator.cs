using System.Collections.Generic;
using UnityEngine;

// Endless procedural platform system. Spawns platform section prefabs in a
// straight line ahead of the player as they roll forward, and destroys
// sections that have fallen behind so the scene doesn't keep growing.
//
// Setup:
// 1. Assign 4+ platform prefabs to "platformPrefabs". Each prefab should be
//    a platform section of the same length ("sectionLength") with its
//    pivot at the section's starting edge (the edge closest to the camera).
// 2. Assign "player" to the rolling ball's Transform.
// 3. Place a few starting sections manually in the scene, or let
//    "initialSectionCount" spawn them automatically on Start.
public class PlatformGenerator : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject[] platformPrefabs; // 4+ different platform section prefabs

    [Header("Generation Settings")]
    public float sectionLength = 20f;        // length of one platform prefab along Z
    public int sectionsAheadToKeep = 6;       // how many sections stay spawned ahead of the player
    public float despawnDistanceBehind = 30f; // how far behind the player before a section is removed

    readonly Queue<GameObject> activeSections = new Queue<GameObject>();
    float nextSpawnZ;

    void Start()
    {
        if (player == null || platformPrefabs == null || platformPrefabs.Length == 0)
        {
            Debug.LogWarning("PlatformGenerator is missing a player reference or platform prefabs.");
            return;
        }

        nextSpawnZ = 0f;

        // Pre-fill the path so the player doesn't spawn staring at empty space.
        for (int i = 0; i < sectionsAheadToKeep; i++)
        {
            SpawnNextSection();
        }
    }

    void Update()
    {
        if (player == null || platformPrefabs == null || platformPrefabs.Length == 0)
        {
            return;
        }

        // Keep spawning ahead as the player approaches the end of the
        // generated track.
        while (nextSpawnZ < player.position.z + sectionsAheadToKeep * sectionLength)
        {
            SpawnNextSection();
        }

        DespawnOldSections();
    }

    void SpawnNextSection()
    {
        GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

        Vector3 spawnPosition = new Vector3(0f, 0f, nextSpawnZ);
        GameObject section = Instantiate(prefab, spawnPosition, Quaternion.identity);

        activeSections.Enqueue(section);
        nextSpawnZ += sectionLength;
    }

    void DespawnOldSections()
    {
        // Sections are spawned in order along Z, so the oldest (front of
        // the queue) is always the one furthest behind the player.
        while (activeSections.Count > 0)
        {
            GameObject oldest = activeSections.Peek();

            if (oldest == null)
            {
                // Already destroyed some other way; just drop it from the queue.
                activeSections.Dequeue();
                continue;
            }

            float sectionEndZ = oldest.transform.position.z + sectionLength;
            bool isFarBehindPlayer = sectionEndZ < player.position.z - despawnDistanceBehind;

            if (isFarBehindPlayer)
            {
                activeSections.Dequeue();
                Destroy(oldest);
            }
            else
            {
                // Queue is ordered, so once we hit one that's not old enough
                // to remove, none of the later ones are either.
                break;
            }
        }
    }
}
