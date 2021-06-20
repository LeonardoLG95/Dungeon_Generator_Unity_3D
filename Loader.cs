using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance == null) Instantiate(gameManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
