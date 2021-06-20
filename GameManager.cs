using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public int playerFoodPoints = 100;
    public static GameManager instance = null;
    private BoardManager boardScript;
    private int level = 3;
    //public GameObject gridManager;
    //private List<Enemy> enemies;
    //private bool enemiesMoving;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        boardScript.DungeonSetup(level);
    }

    public void GameOver()
    {
        enabled = false;
    }
}
