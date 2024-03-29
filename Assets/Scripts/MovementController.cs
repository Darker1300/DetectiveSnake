using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
    public SnakeGrid snakeGrid;
    GameManager gameMan;

    public Vector3Int currentCell;
    public Vector3Int targetCell;
    public bool isMoveFinished = true;

    public float moveTime = 0.5f;

    public bool inputControlled = false;
    public string InputID_X = "Horizontal";
    public string InputID_Y = "Vertical";

    Vector3Int direction = Vector3Int.up;
    Vector2 velocity;

    [HideInInspector] public UnityEvent MoveStart = new UnityEvent();
    [HideInInspector] public UnityEvent MoveUpdate = new UnityEvent();
    [HideInInspector] public UnityEvent MoveFinish = new UnityEvent();

    public Vector3 GetCurrentWorldPos
        => snakeGrid.grid.CellToWorld(currentCell);
    public Vector3 GetTargetWorldPos
        => snakeGrid.grid.CellToWorld(targetCell);

    void Awake()
    {
        snakeGrid = FindObjectOfType<SnakeGrid>();
    }

    void Start()
    {
        currentCell = snakeGrid.grid.WorldToCell(transform.position);
        targetCell = currentCell;
    }

    private void DoMove()
    {
        isMoveFinished = false;

        MoveStart.Invoke();

        currentCell = targetCell;
        targetCell = targetCell + direction;
    }

    void Update()
    {
        if (inputControlled)
        {
            Vector3Int tempDir = Vector3Int.zero;
            tempDir.x = Mathf.RoundToInt(Input.GetAxis(InputID_X));
            tempDir.y = Mathf.RoundToInt(Input.GetAxis(InputID_Y));
            tempDir.z = 0;

            // Only allow vertical or horizontal movement
            if (tempDir != Vector3Int.zero)
            {
                if (Mathf.Abs(tempDir.x) != Mathf.Abs(tempDir.y))
                    direction = tempDir;
            }
        }


        if (!isMoveFinished)
        {
            Vector3 targetWorldPos = snakeGrid.grid.CellToWorld(targetCell);
            transform.position = Vector2.SmoothDamp(
                transform.position,
                targetWorldPos,
                ref velocity,
                moveTime);

            MoveUpdate.Invoke();

            // Check if we reached our target cell
            if ((targetWorldPos - transform.position).sqrMagnitude < 0.001f)
            {
                isMoveFinished = true;
                MoveFinish.Invoke();

                currentCell = targetCell;
            }
        }
    }

    // Draw the movement direction in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(direction.x, direction.y, 0));
    }


    private void OnEnable()
    {
        if (!gameMan) gameMan = FindObjectOfType<GameManager>();
        gameMan.DoMove.AddListener(DoMove);
    }

    private void OnDisable()
    {
        if (!gameMan) gameMan = FindObjectOfType<GameManager>();
        gameMan.DoMove.RemoveListener(DoMove);
    }


}
