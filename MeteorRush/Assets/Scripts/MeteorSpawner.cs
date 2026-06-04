using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;

    public float spawnRate = 3f;

    // Spawn band — lower half of the screen
    public float spawnMinX = -4f;
    public float spawnMaxX = 4f;
    public float spawnMinY = -6f;
    public float spawnMaxY = -2f;

    private float nextSpawnTime = 0f;
    private Transform playerTransform;

    void Start()
    {
        // get the player transform at start
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnMeteor();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnMeteor()
    {
        float x = Random.Range(spawnMinX, spawnMaxX);
        float y = Random.Range(spawnMinY, spawnMaxY);
        Vector3 spawnPos = new Vector3(x, y, 0f);

        GameObject meteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

        Meteor meteorScript = meteor.GetComponent<Meteor>();
        if (meteorScript != null)
            meteorScript.Initialize(playerTransform.position); //create at player transform position
    }
}
