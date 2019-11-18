﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
/// <summary>
/// File Name: Enemy1Controller.cs
/// Author: Kevin Yuayan
/// Last Modified by: Kevin Yuayan
/// Date Last Modified: Nov. 17, 2019
/// Description: Controller for the SnakePlane prefab
/// Revision History:
/// </summary>
public class SnakePlaneController : CollidableObject
{
    private GameController gc;
    private AudioSource explosionSound;

    [Header("Speed Values")]
    [SerializeField]
    public Speed horizontalSpeedRange;
    [SerializeField]
    public Speed verticalSpeedRange;

    public float directionDelay;
    public float fireRate;

    public float verticalSpeed;
    public float horizontalSpeed;

    [SerializeField]
    public Boundary boundary;

    [Header("GameObjects")]
    public GameObject fire;
    public GameObject fireSpawn;

    [Header("Itm spawning")]
    private Vector3 position;
    private int itemChance;
    public GameObject bonusLife;
    public GameObject hpUp;
    public int bonusChancePercentage;
    public int hpUpPercentage;

    private float directionTime = 0.0f;
    private float fireTime = 0.0f;

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
        GameObject gco = GameObject.FindWithTag("GameController");
        gc = gco.GetComponent<GameController>();
        explosionSound = gc.audioSources[(int)SoundClip.EXPLOSION];
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckBounds();
        Fire();
    }

    /// <summary>
    /// This method moves the enemy down the screen by verticalSpeed
    /// </summary>
    void Move()
    {
        directionTime += Time.deltaTime;
        if (directionTime >= directionDelay)
        {
            ChangeDirection();
        }
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
        Vector2 startPosition = new Vector2(Random.Range(boundary.Right, boundary.Right + 2.0f), randomYPosition);
        transform.position = startPosition;

        LookAhead();
    }
    // Changes the plane's vertical speed and rotation. Plane fires a bullet after rotating
    private void ChangeDirection()
    {
        directionTime = 0;
        verticalSpeed *= -1;
        LookAhead();
    }
    void Fire()
    {
        fireTime += Time.deltaTime;
        if(fireTime >= fireRate)
        {
            GameObject fireObj;
            fireObj = Instantiate(fire, fireSpawn.transform.position, Quaternion.identity);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            fireTime = 0;
        }
    }
    // Rotates the enemy to look at the direction of the path
    private void LookAhead()
    {
        Vector3 target = transform.position - new Vector3(horizontalSpeed, verticalSpeed);
        var direction = target - transform.position;
        float angle = (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg) + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
    }


    /// <summary>
    /// This method checks if the enemy reaches the left boundary and then it Resets it. Also keeps the plane in the camera
    /// </summary>
    void CheckBounds()
    {
        if (transform.position.y > boundary.Top)
        {
            ChangeDirection();
        }
        if (transform.position.y < boundary.Bottom)
        {
            ChangeDirection();
        }
        if (transform.position.x <= boundary.Left)
        {
            Reset();
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            // Checks if bullet is from player
            if (!col.GetComponent<FireController>().IsEnemyBullet)
            {
                Destroy(this.gameObject);
                Destroy(col.gameObject);
                gc.Score += 100;
                explosionSound.volume = 0.3f;
                explosionSound.Play();

                //item spawning by enemy dead
                position = this.gameObject.transform.position;
                itemChance = Random.Range(0, 100);
                if (itemChance >= 0 && itemChance <= hpUpPercentage)
                {
                    Instantiate(hpUp, position, Quaternion.identity);
                    Debug.Log("Hp-Up spawned");
                }
                if (itemChance >= 0 && itemChance <= bonusChancePercentage)
                {
                    Instantiate(bonusLife, position, Quaternion.identity);
                    Debug.Log("Bonus spawned");
                }
            }
        }
    }
}