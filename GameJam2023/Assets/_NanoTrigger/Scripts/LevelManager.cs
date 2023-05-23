using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    private LevelManager() { }

    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LevelManager();
            }
            return instance;
        }
    }

    [SerializeField]
    private int currentLevel;

    public int CurrentLevel
    {
        get { return currentLevel; }
        private set { currentLevel = value; }
    }

    public List<Level> levels;

    private List<Level>[] levels2D;

    private void Start()
    {
        SortLevelsInto2DArray();
    }

    public void SortLevelsInto2DArray()
    {
        int maxDepth = levels.Max(level => level.Depth);
        levels2D = new List<Level>[maxDepth + 1];

        for (int i = 0; i <= maxDepth; i++)
        {
            levels2D[i] = new List<Level>();
        }

        foreach (Level level in levels)
        {
            levels2D[level.Depth].Add(level);
        }
    }

    public Level GetLevelByDepthAndIndex(int depth, int index)
    {
        if (depth < 0 || depth >= levels2D.Length || index < 0 || index >= levels2D[depth].Count)
        {
            return null;
        }

        return levels2D[depth][index];
    }

    public void CompleteLevel()
    {
        // Increase the currentLevel
        currentLevel++;

        // Check if there are any levels at the new depth
        if (currentLevel < levels2D.Length && levels2D[currentLevel].Count > 0)
        {
            // Select a random level at the new depth
            int randomIndex = UnityEngine.Random.Range(0, levels2D[currentLevel].Count);
            Level newLevel = levels2D[currentLevel][randomIndex];

            // Instantiate the new level
            Instantiate(newLevel, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.Log("No more levels available at this depth.");
        }
    }
}
