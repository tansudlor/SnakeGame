using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    // Start is called before the first frame update
    public static GameController Instance { get; private set; }
    public Dictionary<Vector2, object> HashMap = new Dictionary<Vector2, object>();

    public List<Vector3> UsedArea = new List<Vector3>();
    public List<Vector3> UnuseArea = new List<Vector3>();

    public static GameController GetInstante()
    {
        if (Instance == null)
        {
            Instance = new GameController();
        }

        return Instance;
    }

    
}
