using UnityEngine;
using System.Collections.Generic;


/// Manages the two-phase delivery loop:
///   Phase 0 (Pickup)  — package is visible, delivery zone is hidden.
///   Phase 1 (Deliver) — package is hidden, delivery zone is visible.
///

public class TargetSpawner : MonoBehaviour
{

    public Transform player;
    public WorldSpawner worldSpawner;



    public GameObject packageObject;

   
    public GameObject deliveryObject;

   
    public TargetArrow arrow;

   
    public float minSpawnDistance = 10f;
    public float collectDistance = 1.5f;

    // 0 = waiting for pickup, 1 = waiting for delivery
    int phase = 0;

    void Start()
    {
        packageObject.SetActive(false);
        deliveryObject.SetActive(false);
        // Small delay so WorldSpawner has time to generate the initial chunks
        Invoke(nameof(SpawnPackage), 0.2f);
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        if (phase == 0 && packageObject.activeSelf)
        {
            CheckCollection(packageObject, OnPackageCollected);
            EnsureTargetChunkLoaded(packageObject);
        }
        else if (phase == 1 && deliveryObject.activeSelf)
        {
            CheckCollection(deliveryObject, OnDeliveryCompleted);
            EnsureTargetChunkLoaded(deliveryObject);
        }
    }

    // ── Spawn helpers ────────────────────────────────────────────────

    void SpawnPackage()
    {
        PlaceOnRoad(packageObject, ignoreDistance: true);
        packageObject.SetActive(true);
        deliveryObject.SetActive(false);
        phase = 0;
        PointArrowAt(packageObject);
    }

    void SpawnDelivery()
    {
        // Pick a road tile that is far from the package (not just the player)
        PlaceOnRoad(deliveryObject, ignoreDistance: false, avoidPosition: packageObject.transform.position);
        deliveryObject.SetActive(true);
        packageObject.SetActive(false);
        phase = 1;
        PointArrowAt(deliveryObject);
    }

    // ── Callbacks ────────────────────────────────────────────────────

    void OnPackageCollected()
    {
        GameManager.Instance.RegisterPickup();
        SpawnDelivery();
    }

    void OnDeliveryCompleted()
    {
        GameManager.Instance.RegisterDelivery();
        SpawnPackage();
    }

    // ── Internals ────────────────────────────────────────────────────

    void CheckCollection(GameObject obj, System.Action callback)
    {
        if (Vector3.Distance(player.position, obj.transform.position) < collectDistance)
        {
            callback?.Invoke();
        }
    }

  
    /// If the chunk containing <paramref name="obj"/> has been unloaded,
    /// respawn the object somewhere that is still loaded.
  
    void EnsureTargetChunkLoaded(GameObject obj)
    {
        Vector2Int coord = worldSpawner.WorldToGrid(obj.transform.position);
        if (!worldSpawner.IsChunkActive(coord))
        {
            PlaceOnRoad(obj, ignoreDistance: false);
        }
    }


    /// Picks a random road-tile world position from all active chunks and
    /// moves <paramref name="obj"/> there.

    void PlaceOnRoad(GameObject obj, bool ignoreDistance, Vector3? avoidPosition = null)
    {
        List<Vector3> candidates = BuildCandidateList(ignoreDistance, avoidPosition);

        if (candidates.Count == 0)
        {
            // Fallback: relax all constraints
            candidates = BuildCandidateList(ignoreDistance: true, avoidPosition: null);
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning("[TargetSpawner] No valid road tile found — retrying in 0.1s.");
            Invoke(nameof(RetrySpawn), 0.1f);
            return;
        }

        obj.transform.position = candidates[Random.Range(0, candidates.Count)];
    }

    List<Vector3> BuildCandidateList(bool ignoreDistance, Vector3? avoidPosition = null)
    {
        List<Vector3> candidates = new List<Vector3>();

        foreach (var kvp in worldSpawner.GetActiveChunks())
        {
            GameObject chunkObj = kvp.Value;
            if (chunkObj == null) continue;

            if (!ignoreDistance &&
                Vector3.Distance(player.position, chunkObj.transform.position) < minSpawnDistance)
                continue;

            // Keep delivery far from the package to avoid trivial deliveries
            if (avoidPosition.HasValue &&
                Vector3.Distance(avoidPosition.Value, chunkObj.transform.position) < minSpawnDistance)
                continue;

            UnityEngine.Tilemaps.Tilemap road =
                chunkObj.transform.Find("Road")?.GetComponent<UnityEngine.Tilemaps.Tilemap>();
            if (road == null) continue;

            foreach (Vector3Int cell in road.cellBounds.allPositionsWithin)
            {
                if (road.HasTile(cell))
                    candidates.Add(road.GetCellCenterWorld(cell));
            }
        }

        return candidates;
    }

    // Fallback retry used when no tiles are ready at spawn time
    bool retryIsDelivery = false;

    void RetrySpawn()
    {
        if (retryIsDelivery) SpawnDelivery();
        else SpawnPackage();
    }

    void PointArrowAt(GameObject obj)
    {
        if (arrow != null)
            arrow.target = obj.transform;
    }
}
