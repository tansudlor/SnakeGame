using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public float DestoryTime = 0f;
    private GameController gameController;
    void Start()
    {
        gameController = GameController.GetInstante();
    }
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject,DestoryTime);
        
    }
    private void OnDestroy()
    {
        Vector2 pos = transform.position;
        gameController.HashMap.Remove(pos);
        gameController.UsedArea.Remove(pos);
        gameController.UnuseArea.Add(pos);
    }
}
