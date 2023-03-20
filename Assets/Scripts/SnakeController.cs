using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private MovementController movement;
    [SerializeField] private int maxSegments = 6;
    [SerializeField] private int numSegments = 1;
    [SerializeField] private List<Vector3> snakeSegments = new(); // Last is head, first is tail

    void Start()
    {
        movement ??= GetComponent<MovementController>();
        lineRenderer ??= GetComponent<LineRenderer>();

        AddHeadSegment(movement.GetCurrentWorldPos);
        UpdateLineRenderer();
    }
    private void OnEnable()
    {
        if (!movement) movement = GetComponent<MovementController>();
        movement.MoveStart.AddListener(OnMoveStart);
        movement.MoveUpdate.AddListener(OnMoveUpdate);
        movement.MoveFinish.AddListener(OnMoveFinish);
    }

    private void OnDisable()
    {
        if (!movement) movement = GetComponent<MovementController>();
        movement.MoveStart.RemoveListener(OnMoveStart);
        movement.MoveUpdate.RemoveListener(OnMoveUpdate);
        movement.MoveFinish.RemoveListener(OnMoveFinish);
    }

    void OnMoveStart()
    {
        // Add the current position to the list of segments
        AddHeadSegment(movement.GetCurrentWorldPos);

        // Remove the first segment if we exceed the max segments
        if (numSegments > maxSegments)
        {
            RemoveTailSegment();
        }

        // Update the line renderer with the new segment positions
        UpdateLineRenderer();
    }

    void OnMoveUpdate()
    {
        UpdateHead();
    }

    void OnMoveFinish()
    {
        //AddHeadSegment(movement.currentCell);

        //UpdateLineRenderer();
    }


    // Add a new segment to the snake
    private void AddHeadSegment(Vector3 worldPos)
    {
        if (snakeSegments.Count > 0)
            snakeSegments[snakeSegments.Count - 1] = worldPos;

        snakeSegments.Add(worldPos);
        numSegments++;
    }

    // Remove the last segment from the snake
    private void RemoveTailSegment()
    {
        if (numSegments <= 1) return;

        snakeSegments.RemoveAt(0);
        numSegments--;
    }


    private void UpdateHead()
    {
        // snakeSegments[snakeSegments.Count - 1] = transform.position;
       lineRenderer.SetPosition(snakeSegments.Count - 1, transform.position);
    }

    private void UpdateLineRenderer()
    {
        UpdateHead();

        // Vector3[] newPoints = Generate_Points(snakeSegments, 100);
        // lineRenderer.positionCount = newPoints.Length;

        lineRenderer.positionCount = numSegments;
        for (int i = 0; i < numSegments; i++)
            lineRenderer.SetPosition(i, snakeSegments[i]);
    }


    Vector3[] Generate_Points(List<Vector3> keyPoints, int segments = 100)
    {
        int keyCount = keyPoints.Count;
        Vector3[] Points = new Vector3[(keyCount - 1) * segments + keyCount];
        for (int i = 1; i < keyCount; i++)
        {
            Points[(i - 1) * segments + i - 1] = new Vector3(keyPoints[i - 1].x, keyPoints[i - 1].y, 0);
            for (int j = 1; j <= segments; j++)
            {
                float x = keyPoints[i - 1].x;
                float y = keyPoints[i - 1].y;
                float z = 0;//keyPoints [i - 1].z;
                float dx = (keyPoints[i].x - keyPoints[i - 1].x) / segments;
                float dy = (keyPoints[i].y - keyPoints[i - 1].y) / segments;
                Points[(i - 1) * segments + j + i - 1] = new Vector3(x + dx * j, y + dy * j, z);
            }
        }
        Points[(keyCount - 1) * segments + keyCount - 1] = new Vector3(keyPoints[keyCount - 1].x, keyPoints[keyCount - 1].y, 0);
        return Points;
    }


    //private void UpdateLastSegment(Vector3 worldPos)
    //{
    //    if (lineRenderer.positionCount < 1) return;

    //    lineRenderer.SetPosition(lineRenderer.positionCount - 1, worldPos);
    //}

}