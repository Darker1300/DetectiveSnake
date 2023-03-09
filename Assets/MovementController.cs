using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.Mathematics;
using UnityEditor.Build.Content;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //float speed = 5f;
    Vector3Int currentCell;
    Vector3Int targetCell;
    SnakeGrid snakeGrid;
    GameManager gameMan;

    public float moveTime = 0.5f;

    public bool inputControlled = false;
    public string InputID_X = "Horizontal";
    public string InputID_Y = "Vertical";

    Vector3Int direction = Vector3Int.up;
    Vector2 velocity;

    void Start()
    {
        gameMan = FindObjectOfType<GameManager>();
        snakeGrid = FindObjectOfType<SnakeGrid>();
        currentCell = snakeGrid.grid.WorldToCell(transform.position);
        targetCell = currentCell;

    }

    void Update()
    {
        if (inputControlled)
        {
            Vector3Int tempDir = Vector3Int.zero;
            tempDir.x = Mathf.RoundToInt(Input.GetAxis(InputID_X));
            tempDir.y = Mathf.RoundToInt(Input.GetAxis(InputID_Y));
            tempDir.z = 0;

            if (tempDir != Vector3Int.zero)
                direction = tempDir;
        }

        transform.position = Vector2.SmoothDamp(transform.position, snakeGrid.grid.CellToWorld(targetCell), ref velocity, moveTime);
    }

    private void OnMove()
    {
        Vector3Int newTarget = targetCell + direction;
        if (math.abs(newTarget.x) > snakeGrid.gridSize)
        {
            newTarget.x = targetCell.x - direction.x;
        }
        if (math.abs(newTarget.y) > snakeGrid.gridSize)
        {
            newTarget.y = targetCell.y - direction.y;
        }
        targetCell = newTarget;
    }

    private void OnEnable()
    {
        if (!gameMan) gameMan = FindObjectOfType<GameManager>();
        gameMan.DoMove.AddListener(OnMove);
    }

    private void OnDisable()
    {
        if (!gameMan) gameMan = FindObjectOfType<GameManager>();
        gameMan.DoMove.RemoveListener(OnMove);
    }


}
