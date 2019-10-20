using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// File Name: BackgroundController.cs
/// Author: Philip Lee
/// Last Modified by: Philip Lee
/// Date Last Modified: Oct. 4, 2019
/// Description: Controller for the Background prefab
/// Revision History:
/// </summary>
public class BackgroundController : MonoBehaviour
{
    public float horizontalSpeed;
    public float resetPostion;
    public float resetPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckBound();
    }

    void Move()
    {
        Vector2 newPosition = new Vector2(horizontalSpeed, 0.0f);
        Vector2 currentPosition = transform.position;
        currentPosition -= newPosition;
        transform.position = currentPosition;
    }
    void Reset()
    {
        transform.position = new Vector2(resetPostion, 0.0f);
    }
    void CheckBound()
    {
        if(transform.position.x <= resetPoint)
        {
            Reset();
        }
    }
}
