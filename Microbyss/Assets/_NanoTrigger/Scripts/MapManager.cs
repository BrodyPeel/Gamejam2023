using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2 Position;
    public GameObject TileObject; // Physical representation of the tile.
}

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    private bool isUpdatingTiles = false;


    public GameObject tilePrefab; // The tile prefab.
    public GameObject[] objectPrefabs; // Prefabs for objects to place on tiles
    public GameObject player; // Reference to the player GameObject.
    public int tileUpdateDistance = 5; // How close the player needs to be to update tiles.
    public int tilePersistenceDistance = 10; // Distance from player before tiles are culled.
    public float tileSize = 1f; // Distance between tiles, aligning with Unity units
    public float maxDistanceFromCenter = 5f;

    private List<Tile> tiles = new List<Tile>(); // Dynamic list of current tiles.
    private List<Collider2D> globalObjectColliders = new List<Collider2D>(); // Global registry for collision detection.

    private void Awake()
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

    void Update()
    {
        if (GameManager.Instance.state.isState("PlayState") && !isUpdatingTiles)
        {
            StartCoroutine(UpdateTilesCoroutine(player.transform.position));
        }
    }

    IEnumerator UpdateTilesCoroutine(Vector3 playerPosition)
    {
        isUpdatingTiles = true;
        yield return StartCoroutine(UpdateMapTiles(playerPosition));
        isUpdatingTiles = false;
    }

    IEnumerator UpdateMapTiles(Vector3 playerPosition)
    {
        // Calculate the player's tile position
        Vector2 playerTilePos = new Vector2(Mathf.Round(playerPosition.x / tileSize), Mathf.Round(playerPosition.y / tileSize));

        // Iterate through tiles to update
        List<Vector2> tilePositionsToUpdate = new List<Vector2>();
        for (int x = -tileUpdateDistance; x <= tileUpdateDistance; x++)
        {
            for (int y = -tileUpdateDistance; y <= tileUpdateDistance; y++)
            {
                Vector2 tilePos = playerTilePos + new Vector2(x, y);
                if (!TileExistsAt(tilePos))
                {
                    tilePositionsToUpdate.Add(tilePos);
                }
            }
            yield return null;
        }

        // Sort the tile positions based on their distance from the player
        tilePositionsToUpdate.Sort((a, b) => Vector2.Distance(playerTilePos, a).CompareTo(Vector2.Distance(playerTilePos, b)));

        // Generate/update tiles and cull distant tiles
        foreach (Vector2 tilePos in tilePositionsToUpdate)
        {
            GenerateTileAt(tilePos);
            yield return null;
        }

        CullDistantTiles(playerTilePos);
    }



    void CullDistantTiles(Vector2 playerTilePos)
    {
        for (int i = tiles.Count - 1; i >= 0; i--)
        {
            if (!IsTileWithinUpdateDistance(tiles[i].Position, playerTilePos))
            {
                // Before removing a tile, also remove its objects from the global registry.
                var tileColliders = tiles[i].TileObject.GetComponentsInChildren<Collider2D>();
                foreach (var collider in tileColliders)
                {
                    globalObjectColliders.Remove(collider);
                }

                Destroy(tiles[i].TileObject);
                tiles.RemoveAt(i);
            }
        }
    }

    bool IsTileWithinUpdateDistance(Vector2 tilePosition, Vector2 playerTilePos)
    {
        return Vector2.Distance(tilePosition, playerTilePos) <= tileUpdateDistance;
    }

    bool TileExistsAt(Vector2 position)
    {
        Vector2 roundedPosition = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        foreach (Tile tile in tiles)
        {
            Vector2 tileRoundedPosition = new Vector2(Mathf.Round(tile.Position.x), Mathf.Round(tile.Position.y));
            if (tileRoundedPosition == roundedPosition)
            {
                return true;
            }
        }
        return false;
    }

    void GenerateTileAt(Vector2 position)
    {
        Vector2 roundedPosition = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
        if (TileExistsAt(roundedPosition))
        {
            // Early return if the tile already exists.
            return;
        }

        Vector3 worldPosition = new Vector3(roundedPosition.x * tileSize, roundedPosition.y * tileSize, 0);
        GameObject tileObject = Instantiate(tilePrefab, worldPosition, Quaternion.identity);
        tileObject.name = "Tile " + roundedPosition;
        Tile newTile = new Tile { Position = roundedPosition, TileObject = tileObject };
        tiles.Add(newTile);

        // Check if the tile is at (0, 0). If not, proceed to spawn objects on it.
        if (roundedPosition != Vector2.zero) // Vector2.zero is shorthand for (0, 0)
        {
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(PlaceObjectsWithDelay(tileObject));
            }
        }
    }

    IEnumerator PlaceObjectsWithDelay(GameObject tileObject)
    {
        if (tileObject == null) yield break;

        //for (int i = 0; i < 3; i++) // For placing 3 objects
        //{
            // Check again in case tileObject was destroyed during the coroutine wait
            if (tileObject == null) yield break;
            yield return StartCoroutine(AttemptToPlaceObjectInTile(tileObject, 0));
        //}
    }

    IEnumerator AttemptToPlaceObjectInTile(GameObject tileObject, int attemptCount)
    {
        if (tileObject == null || attemptCount > 5)
        {
            //Debug.LogWarning("Attempt to place object in tile was aborted due to null tileObject or excessive attempts.");
            yield break;
        }

        Vector2 randomPositionWithinTile = (Vector2)tileObject.transform.position + new Vector2(Random.Range(-maxDistanceFromCenter, maxDistanceFromCenter), Random.Range(-maxDistanceFromCenter, maxDistanceFromCenter));

        GameObject newObj = Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Length)], randomPositionWithinTile, Quaternion.identity, tileObject.transform);

        yield return null; // Wait for the next frame to let the physics engine update.

        if (newObj == null) yield break; // Safety check after yield

        PolygonCollider2D newCollider = newObj.GetComponent<PolygonCollider2D>();
        if (newCollider == null || newObj == null) // Check newObj again in case it got destroyed right after instantiation
        {
            //Debug.LogError("New object does not have a PolygonCollider2D attached, or the object was destroyed.");
            if (newObj != null) Destroy(newObj); // Cleanup if the object isn't suitable
            yield break;
        }

        // Check for overlaps with the global registry
        bool overlapFound = false;
        foreach (var collider in globalObjectColliders)
        {
            if (collider == null) continue; // Skip any null entries in the global registry

            // Ensure newObj hasn't been destroyed before accessing its properties
            if (newObj == null || newCollider == null) yield break;

            if (collider.bounds.Intersects(newCollider.bounds))
            {
                overlapFound = true;
                break; // Found an overlap, no need to check further
            }
        }

        if (overlapFound)
        {
            // If there's an overlap, destroy the new object
            if (newObj != null) // Double-check newObj isn't null before attempting to destroy it
                Destroy(newObj);
        }
        else
        {
            // If no overlap, add the object's collider to the global registry
            // Ensure newObj and its collider haven't been destroyed before adding to the registry
            if (newObj != null && newCollider != null)
                globalObjectColliders.Add(newCollider);
        }
    }

    float CalculateColliderArea(PolygonCollider2D collider)
    {
        float area = 0f;
        Vector2[] points = collider.points;
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 point1 = points[i];
            Vector2 point2 = points[(i + 1) % points.Length];
            area += (point1.x * point2.y - point2.x * point1.y) / 2f;
        }
        return Mathf.Abs(area); // Ensure the area is positive
    }
}