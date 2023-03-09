using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGrid : MonoBehaviour
{
    public Grid grid;
    public int gridSize = 10;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        
    }
}
