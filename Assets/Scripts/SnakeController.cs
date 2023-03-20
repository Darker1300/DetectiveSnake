using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private MovementController movement;
    [SerializeField] private int maxSegments = 3;
    [SerializeField] private int numSegments = 0;
    [SerializeField] private List<Vector3> snakeSegments = new();

    void Start()
    {
        movement ??= GetComponent<MovementController>();
        lineRenderer ??= GetComponent<LineRenderer>();

        AddSegment();
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

    void OnMoveStart(Vector3 currentWorldPos)
    {
    }

    void OnMoveUpdate(Vector3 targetWorldPos)
    {
        UpdateLastSegment();
    }

    void OnMoveFinish(Vector3 targetWorldPos)
    {
        // Add the current position to the list of segments
        snakeSegments.Add(targetWorldPos);
        numSegments++;

        // Remove the first segment if we exceed the max segments
        if (numSegments > maxSegments)
        {
            snakeSegments.RemoveAt(0);
            numSegments--;
        }

        // Update the line renderer with the new segment positions
        UpdateLineRenderer();
    }


    // Add a new segment to the snake
    private void AddSegment()
    {
        if (numSegments >= maxSegments)
            return;

        Vector3 currentWorldPos = movement.GetCurrentWorldPos;
        currentWorldPos.z = transform.position.z;
        snakeSegments.Add(currentWorldPos);
        numSegments++;

        // Update the line renderer with the new segment positions
        UpdateLineRenderer();
    }

    // Remove the last segment from the snake
    private void RemoveSegment()
    {
        if (numSegments <= 1) return;

        snakeSegments.RemoveAt(numSegments - 1);
        numSegments--;

        // Update the line renderer with the new segment positions
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = numSegments;
        for (int i = 0; i < numSegments; i++)
            lineRenderer.SetPosition(i, snakeSegments[i]);
    }

    private void UpdateLastSegment()
    {
        if (lineRenderer.positionCount < 1) return;

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);
    }

}