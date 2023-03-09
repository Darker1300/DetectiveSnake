using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public bool canMove = true;
    public float timer = 0f;
    public float moveTime = 0.75f;

    public UnityEvent DoMove = new UnityEvent();

    void Start()
    {
        
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (canMove && timer <= 0f)
        {
            DoMove.Invoke();
            timer += moveTime;
        }
    }
}
