using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

/// <summary>
/// File Name: Enemy1Controller.cs
/// Author: Kevin Yuayan
/// Last Modified by: Kevin Yuayan
/// Date Last Modified: Oct. 2, 2019
/// Description: Controller for the Enemy_1 prefab
/// Revision History:
/// </summary>
public class Enemy1Controller : CollidableObject
{
    [Header("Speed Values")]
    [SerializeField]
    public Speed horizontalSpeedRange;

    [SerializeField]
    public Speed verticalSpeedRange;

    public float verticalSpeed;
    public float horizontalSpeed;

    [SerializeField]
    public Boundary boundary;

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
        horizontalSpeed = Random.Range(horizontalSpeedRange.min, horizontalSpeedRange.max);
        verticalSpeed = Random.Range(verticalSpeedRange.min, verticalSpeedRange.max);

        float randomYPosition = Random.Range(boundary.Bottom, boundary.Top);
        Vector2 startPosition = new Vector2(Random.Range(boundary.Right, boundary.Top + 2.0f), randomYPosition );
        transform.position = startPosition;

        // Rotates the enemy to look at the direction of the path

        Vector2 target = startPosition - new Vector2(horizontalSpeed, verticalSpeed);
        var direction = target - startPosition;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
    }

    /// <summary>
    /// This method checks if the enemy reaches the lower boundary and then it Resets it
    /// </summary>
    void CheckBounds()
    {
        if (transform.position.x <= boundary.Left)
        {
            Reset();
        }
    }
}
