using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;
using static UnityEditor.PlayerSettings;

public class Snake : MonoBehaviour
{
    public GameObject SnakePartPrefab;
    public GameObject FoodPrefab;
    private Vector3 direction = Vector3.right;

    private Dictionary<Vector2, object> hashMap = new Dictionary<Vector2, object>();
    public float speed = 1f;
    private int BodyCount = 1;
    private Vector3 position;

    private GameController gameController;
    public List<Vector3> usedArea = new List<Vector3>();
    public List<Vector3> unuseArea = new List<Vector3>();

    private List<Vector3> TailPos = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.GetInstante();
        gameController.HashMap = hashMap;
        gameController.UnuseArea = unuseArea;
        gameController.UsedArea = usedArea;
        direction = Vector2.up;
        position = Vector2.zero;
        for (int i = -16; i <= 15; i++)//X Pos
        {
            for (int j = -8; j <= 9; j++)//Y Pos
            {
                Vector2 pos = new Vector2();
                pos.x = i;
                pos.y = j;
                unuseArea.Add(pos);

            }

        }

        SpawnFood();
        StartCoroutine(GameInterval());
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

    }

    IEnumerator GameInterval()
    {
        while (true)
        {
            MoveSnake();
            yield return new WaitForSeconds(speed);
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector3.down) direction = Vector3.up;
        if (Input.GetKeyDown(KeyCode.S) && direction != Vector3.up) direction = Vector3.down;
        if (Input.GetKeyDown(KeyCode.A) && direction != Vector3.right) direction = Vector3.left;
        if (Input.GetKeyDown(KeyCode.D) && direction != Vector3.left) direction = Vector3.right;
    }

    void MoveSnake()
    {

        position += direction;
        if (position.x > 15)
        {
            position.x = -16;
        }
        if (position.x < -16)
        {
            position.x = 15;
        }
        if (position.y > 9)
        {
            position.y = -8;
        }
        if (position.y < -8)
        {
            position.y = 9;
        }
        var blockX = position.x;
        var blockY = position.y;

       

        //position.x = blockX;
        Vector2 currentBlock = new Vector2(blockX, blockY);

        if (!hashMap.ContainsKey(currentBlock))
        {

            CreateTail(currentBlock, speed * BodyCount);


        }
        else
        {

            if (hashMap[currentBlock].GetType() == typeof(Tail))
            {
                Time.timeScale = 0;
                Debug.Log("Die");
            }
            else if (hashMap[currentBlock].GetType() == typeof(Food))
            {
                
                DestoryFood(currentBlock);
                CreateTail(currentBlock, speed * BodyCount);
                SpawnFood();
                BodyCount++;
                

            }
        }
       
        //Debug.Log("BodyCount " + BodyCount);

    }

    void CreateTail(Vector2 currentBlock, float destoryTime)
    {
        var snake = Instantiate(SnakePartPrefab, new Vector3(currentBlock.x, currentBlock.y, 0), Quaternion.identity);
        snake.gameObject.name = currentBlock.x + " : " + currentBlock.y;
        var TailScript = snake.GetComponent<Tail>();
        TailScript.DestoryTime = destoryTime;
        hashMap[currentBlock] = TailScript;
        unuseArea.Remove(currentBlock);
        usedArea.Add(currentBlock);
    }
   
    void DestoryFood(Vector2 pos)
    {
        var food = (Food)hashMap[pos];
        Destroy(food.gameObject);
        hashMap.Remove(pos);
        usedArea.Remove(pos);
        unuseArea.Add(pos);
    }

    void SpawnFood()
    {
        int randPos = Random.Range(0, unuseArea.Count);
        Vector3 usePos = unuseArea[randPos];
        usedArea.Add(usePos);
        unuseArea.Remove(usePos);

        var food = Instantiate(FoodPrefab, usePos, Quaternion.identity).GetComponent<Food>();
        hashMap[usePos] = food;
    }

}
