using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int level = 3;

    public static GameManager instance = null;
    private MazeCreator mazeCreator;
    //public GameObject gridManager;
    //private List<Enemy> enemies;
    //private bool enemiesMoving;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        mazeCreator = GetComponent<MazeCreator>();
        mazeCreator.Create(level);
    }

    public void GameOver()
    {
        enabled = false;
    }
}
