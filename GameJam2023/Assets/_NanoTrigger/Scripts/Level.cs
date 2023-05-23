using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private int clearCount;

    public int ClearCount
    {
        get { return clearCount; }
        private set { clearCount = value; }
    }

    [SerializeField]
    private int depth;

    public int Depth
    {
        get { return depth; }
        private set { depth = value; }
    }
}
