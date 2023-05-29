using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public Level currentLevel;

    [SerializeField]
    private int currentDepth;

    public int CurrentDepth
    {
        get { return currentDepth; }
        private set { currentDepth = value; }
    }

    public List<GameObject> levelPrefabs; // For storing level GameObject prefabs

    private List<Level>[] levels2D;
    private Dictionary<Level, GameObject> levelToPrefab = new Dictionary<Level, GameObject>(); // Map each level to its GameObject

    private void Start()
    {
        SortLevelsInto2DArray();

        // Populate levelToPrefab dictionary
        for (int i = 0; i < levels2D.Length; i++)
        {
            for (int j = 0; j < levels2D[i].Count; j++)
            {
                Level level = levels2D[i][j];
                GameObject prefab = levelPrefabs.First(p => p.GetComponent<Level>() == level);

                if (prefab != null)
                {
                    levelToPrefab[level] = prefab;
                }
                else
                {
                    Debug.LogError($"Prefab for level {level.name} not found");
                }
            }
        }

        // Pick Random Entry Level
        Level entryLevel = GetRandomLevelByDepth(0);

        // Get Level Prefab
        GameObject entryLevelPrefab = GetPrefabByLevel(entryLevel);

        GameObject levelObject = Instantiate(entryLevelPrefab);

        currentLevel = levelObject.GetComponent<Level>();
    }


    public void SortLevelsInto2DArray()
    {
        int maxDepth = levelPrefabs.Max(prefab => prefab.GetComponent<Level>().Depth);
        levels2D = new List<Level>[maxDepth + 1];

        for (int i = 0; i <= maxDepth; i++)
        {
            levels2D[i] = new List<Level>();
        }

        for (int i = 0; i < levelPrefabs.Count; i++)
        {
            Level level = levelPrefabs[i].GetComponent<Level>();
            levels2D[level.Depth].Add(level);
        }
    }


    public Level GetRandomLevelByDepth(int depth)
    {
        if (depth < 0 || depth >= levels2D.Length || levels2D[depth].Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, levels2D[depth].Count);
        return levels2D[depth][randomIndex];
    }

    public Level GetLevelByDepthAndIndex(int depth, int index)
    {
        if (depth < 0 || depth >= levels2D.Length || index < 0 || index >= levels2D[depth].Count)
        {
            return null;
        }

        return levels2D[depth][index];
    }

    public GameObject GetPrefabByLevel(Level level)
    {
        if (levelToPrefab.TryGetValue(level, out GameObject prefab))
        {
            return prefab;
        }

        Debug.LogWarning("Level does not have a corresponding prefab. Returning null.");
        return null;
    }

    public void CompleteLevel()
    {
        if (currentLevel.isBoss)
        {
            GameManager.Instance.camera.target = GameManager.Instance.levelManager.currentLevel.cameraContainer.transform;
            GameManager.Instance.procedureSuccess = true;
            GameManager.Instance.state.ChangeState(GameManager.Instance.state.resultState);
        }
        else
        {
            // Increase the currentLevel
            currentDepth++;

            // Check if there are any levels at the new depth
            if (currentDepth < levels2D.Length && levels2D[currentDepth].Count > 0)
            {
                // Select a random level at the new depth
                int randomIndex = UnityEngine.Random.Range(0, levels2D[currentDepth].Count);
                Level newLevel = levels2D[currentDepth][randomIndex];

                // Instantiate the new level
                GameObject newLevelPrefab = levelToPrefab[newLevel];
                GameObject newLevelObject = Instantiate(newLevelPrefab, Vector3.zero, Quaternion.identity);
                currentLevel = newLevelObject.GetComponent<Level>();

                GameManager.Instance.state.ChangeState(GameManager.Instance.state.enterLevelState);
            }
            else
            {
                Debug.Log("No more levels available at this depth.");
            }
        }
    }
}
