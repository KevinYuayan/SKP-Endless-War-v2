using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

/// <summary>
/// File Name: Enemy2Controller.cs
/// Author: Kevin Yuayan
/// Last Modified by: Kevin Yuayan
/// Date Last Modified: Oct. 2, 2019
/// Description: Controller for the Enemy_2 prefab
/// Revision History:
/// </summary>
public class Enemy2Controller : CollidableObject
{
    public float verticalSpeed;
    public float horizontalSpeed;

    [SerializeField]
    public Boundary boundary;

    // private instance variables
    private Vector2 target;

    // public properties
    public override bool HasCollided
    {
        get
        {
            return _hasCollided;
        }
        set
        {
            _hasCollided = value;
            if (HasCollided)
            {
                Reset();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckBounds();
    }

    /// <summary>
    /// This method moves the enemy down the screen by verticalSpeed
    /// </summary>
    void Move()
    {
        Vector2 newPosition = new Vector2(horizontalSpeed, verticalSpeed);
        Vector2 currentPosition = transform.position;

        currentPosition -= newPosition;
        transform.position = currentPosition;
    }

    /// <summary>
    /// This method resets the enemy to a random resetPosition and randomly changes their speeds
    /// </summary>
    public override void Reset()
    {
        base.Reset();
        // Sets the new position
        float randomYPosition = Random.Range(boundary.Bottom, boundary.Top);
        Vector2 startPosition = new Vector2(Random.Range(boundary.Right, boundary.Right + 2.0f), randomYPosition );
        transform.position = startPosition;

        // Sets the speeds based on the position of the player
        target = GameObject.Find("Player").transform.position;
        horizontalSpeed = (transform.position.x - target.x) / 70;
        verticalSpeed = (transform.position.y - target.y) / 70;

        // Rotates the enemy to look at the player's position
        var direction = target - startPosition;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
    }
    /// <summary>
    /// This method checks if the enemy reaches the lower boundary and then it Resets it
    /// </summary>
    void CheckBounds()
    {
        if (transform.position.y >= boundary.Top)
        {
            Reset();
        }
        if (transform.position.y <= boundary.Bottom)
        {
            Reset();
        }
        if (transform.position.x <= boundary.Left)
        {
            Reset();
        }
    }
}
