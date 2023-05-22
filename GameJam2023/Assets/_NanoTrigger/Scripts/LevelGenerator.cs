
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int width, height;
    public GameObject[] roomPrefabs;
    public GameObject corridorPrefab;
    public GameObject doorPrefab;
    public GameObject lockedDoorPrefab; //turned off
    public GameObject bossRoomPrefab;
    public GameObject secretRoomPrefab; //turned off
    public GameObject shopRoomPrefab; //turned off
    public GameObject stairsPrefab; //turned off
    public GameObject[] enemies;
    public GameObject[] items;

    private int[,] grid;
    private List<Vector2Int> walls;

    private List<Vector2Int> mainPath;


    void Start()
    {
        grid = new int[width, height];
        walls = new List<Vector2Int>();
        GenerateLevel();
        InstantiateLevel();
    }

    void GenerateLevel()
    {
        mainPath = new List<Vector2Int>();
        Vector2Int start = new Vector2Int(width / 2, height / 2);
        grid[start.x, start.y] = 1;
        mainPath.Add(start);

        AddWalls(start);

        while (walls.Count > 0)
        {
            int randomIndex = Random.Range(0, walls.Count);
            Vector2Int currentWall = walls[randomIndex];
            walls.RemoveAt(randomIndex);

            if (IsValidWall(currentWall))
            {
                Vector2Int opposite = GetOpposite(currentWall);

                if (grid[opposite.x, opposite.y] == 0)
                {
                    grid[currentWall.x, currentWall.y] = 1;
                    grid[opposite.x, opposite.y] = 1;

                    if (mainPath.Contains(currentWall - (opposite - currentWall)))
                    {
                        mainPath.Add(opposite);
                    }

                    AddWalls(opposite);
                }
            }
        }
    }


    void AddWalls(Vector2Int position)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int newPos = position + direction;

            if (newPos.x > 0 && newPos.x < width - 1 && newPos.y > 0 && newPos.y < height - 1)
            {
                if (grid[newPos.x, newPos.y] == 0)
                {
                    walls.Add(newPos);
                }
            }
        }
    }

    bool IsValidWall(Vector2Int position)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        int adjacentRooms = 0;

        foreach (Vector2Int direction in directions)
        {
            Vector2Int newPos = position + direction;

            if (newPos.x >= 0 && newPos.x < width && newPos.y >= 0 && newPos.y < height)
            {
                if (grid[newPos.x, newPos.y] == 1)
                {
                    adjacentRooms++;
                }
            }
        }

        return adjacentRooms == 1;
    }

    Vector2Int GetOpposite(Vector2Int position)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int newPos = position + direction;

            if (newPos.x >= 0 && newPos.x < width && newPos.y >= 0 && newPos.y < height)
            {
                if (grid[newPos.x, newPos.y] == 1)
                {
                    return position + direction * 2;
                }
            }
        }

        return Vector2Int.zero;
    }

    void InstantiateLevel()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 1)
                {
                    int randomRoomIndex = Random.Range(0, roomPrefabs.Length);
                    Instantiate(roomPrefabs[randomRoomIndex], new Vector3(x, y, 0), Quaternion.identity, transform);

                    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right };
                    foreach (Vector2Int direction in directions)
                    {
                        Vector2Int adjacent = new Vector2Int(x + direction.x, y + direction.y);
                        if (adjacent.x < width && adjacent.y < height && grid[adjacent.x, adjacent.y] == 1)
                        {
                            if (mainPath.Contains(new Vector2Int(x, y)) && mainPath.Contains(adjacent))
                            {
                                Instantiate(doorPrefab, new Vector3(x + direction.x / 2f, y + direction.y / 2f, 0), Quaternion.identity, transform);
                            }
                            /*else if (Random.value < 0.1f)
                            {
                                Instantiate(lockedDoorPrefab, new Vector3(x + direction.x / 2f, y + direction.y / 2f, 0), Quaternion.identity, transform);
                            }
                            else
                            {
                                Instantiate(doorPrefab, new Vector3(x + direction.x / 2f, y + direction.y / 2f, 0), Quaternion.identity, transform);
                            }*/
                        }
                    }
                }
                else
                {
                    Instantiate(corridorPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                }
            }
        }

        Vector2Int bossRoomPosition = PlaceBossRoom();
        Instantiate(bossRoomPrefab, new Vector3(bossRoomPosition.x, bossRoomPosition.y, 0), Quaternion.identity, transform);

        //CreateSpecialRooms();

        //Instantiate(stairsPrefab, new Vector3(width / 2, height / 2, 0), Quaternion.identity, transform);

        SpawnObjects();
    }

    Vector2Int PlaceBossRoom()
    {
        Vector2Int furthestRoom = new Vector2Int(width / 2, height / 2);
        float maxDistance = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == 1)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(width / 2, height / 2));
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        furthestRoom = new Vector2Int(x, y);
                    }
                }
            }
        }

        return furthestRoom;
    }

    void CreateSpecialRooms()
    {
        // Create a secret room
        Vector2Int secretRoomPosition = PlaceSecretRoom();
        Instantiate(secretRoomPrefab, new Vector3(secretRoomPosition.x, secretRoomPosition.y, 0), Quaternion.identity, transform);

        // Create a shop room
        Vector2Int shopRoomPosition = PlaceShopRoom();
        Instantiate(shopRoomPrefab, new Vector3(shopRoomPosition.x, shopRoomPosition.y, 0), Quaternion.identity, transform);
    }

    Vector2Int PlaceSecretRoom()
    {
        Vector2Int secretRoomPosition;
        do
        {
            secretRoomPosition = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1));
        } while (grid[secretRoomPosition.x, secretRoomPosition.y] != 0);

        return secretRoomPosition;
    }

    Vector2Int PlaceShopRoom()
    {
        Vector2Int shopRoomPosition;
        do
        {
            shopRoomPosition = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1));
        } while (grid[shopRoomPosition
        .x, shopRoomPosition.y] != 1);

        return shopRoomPosition;
    }

    void SpawnObjects()
    {
        foreach (Transform room in transform)
        {
            // Check if the child object is a room, not a corridor or door
            if (room.CompareTag("Room"))
            {
                // Spawn enemies
                int enemyCount = Random.Range(1, 4);
                for (int i = 0; i < enemyCount; i++)
                {
                    Vector2 spawnPosition = new Vector2(Random.Range(room.position.x - 0.5f, room.position.x + 0.5f), Random.Range(room.position.y - 0.5f, room.position.y + 0.5f));
                    int randomEnemyIndex = Random.Range(0, enemies.Length);
                    Instantiate(enemies[randomEnemyIndex], spawnPosition, Quaternion.identity, room);
                }

                // Spawn items
                int itemCount = Random.Range(0, 2);
                for (int i = 0; i < itemCount; i++)
                {
                    Vector2 spawnPosition = new Vector2(Random.Range(room.position.x - 0.5f, room.position.x + 0.5f), Random.Range(room.position.y - 0.5f, room.position.y + 0.5f));
                    int randomItemIndex = Random.Range(0, items.Length);
                    Instantiate(items[randomItemIndex], spawnPosition, Quaternion.identity, room);
                }
            }
        }
    }
}
