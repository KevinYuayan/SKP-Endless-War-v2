﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

/// <summary>
/// File Name: BomberController.cs
/// Author: Kevin Yuayan
/// Last Modified by: Kevin Yuayan
/// Date Last Modified: Oct. 19, 2019
/// Description: Controller for the Bomber prefab Bomber only spawns on the top
/// or bottom of the screen. shoots at a set angle
/// Revision History:
/// </summary>
public class BomberController : CollidableObject
{
    private GameController gc;
    private AudioSource explosionSound;

    [Header("Speed Values")]
    [SerializeField]
    public Speed horizontalSpeedRange;
    
    public float horizontalSpeed;

    [SerializeField]
    public Boundary topBoundary;
    [SerializeField]
    public Boundary bottomBoundary;
    [SerializeField]
    private bool _isTopBomber;

    public GameObject fireSpawnBottom;
    public GameObject fireSpawnTop;
    public GameObject fire;
    public float fireRate;
    private float myTime = 0.0f;

    [Header("Item spawning")]
    private Vector3 position;
    private int itemChance1;
    private int itemChance2;
    private int itemChance3;
    public GameObject bonusLife;
    public GameObject powerUP;
    public GameObject hpUp;

    [Header("Item spawning chance")]
    public int hpUpPercentage;
    public int bonusChancePercentage;
    public int powerUpChancePercentage;
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
        _isTopBomber = (Random.Range(0, 1) < 0.5);
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

    void Fire()
    {
        myTime += Time.deltaTime;

        if (myTime > fireRate)
        {
            GameObject fireObj;
            if (_isTopBomber)
            {
                fireObj = Instantiate(fire, fireSpawnBottom.transform.position, fireSpawnBottom.transform.rotation);
            }
            else
            {
                fireObj = Instantiate(fire, fireSpawnTop.transform.position, fireSpawnTop.transform.rotation);
            }            
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            myTime = 0.0f;
            //fireSound.volume = 0.3f;
            //fireSound.Play();
        }
    }

    /// <summary>
    /// This method moves the enemy down the screen by verticalSpeed
    /// </summary>
    void Move()
    {
        Vector2 newPosition = new Vector2(horizontalSpeed, 0);
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
        _isTopBomber = !_isTopBomber;
        horizontalSpeed = Random.Range(horizontalSpeedRange.min, horizontalSpeedRange.max);
        Vector2 startPosition = new Vector2();
        if (_isTopBomber)
        {
            float randomYPosition = Random.Range(topBoundary.Bottom, topBoundary.Top);
            startPosition = new Vector2(Random.Range(topBoundary.Right, topBoundary.Right + 2.0f), randomYPosition);
            transform.position = startPosition;
        }
        else
        {
            float randomYPosition = Random.Range(bottomBoundary.Bottom, bottomBoundary.Top);
            startPosition = new Vector2(Random.Range(bottomBoundary.Right, bottomBoundary.Right + 2.0f), randomYPosition);
            transform.position = startPosition;
        }
        // Rotates the enemy to look at the direction of the path

        Vector2 target = startPosition - new Vector2(horizontalSpeed, 0);
        var direction = target - startPosition;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.back);
    }

    /// <summary>
    /// This method checks if the enemy reaches the lower boundary and then it Resets it
    /// </summary>
    void CheckBounds()
    {
        if (transform.position.x <= topBoundary.Left)
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
                if (col.gameObject.tag == "Bullet")
                {
                    position = this.gameObject.transform.position;
                    Destroy(this.gameObject);
                    Destroy(col.gameObject);
                    gc.Score += 100;
                    explosionSound.volume = 0.3f;
                    explosionSound.Play();
                    itemChance1 = Random.Range(0, 100);
                    itemChance2 = Random.Range(0, 100);
                    itemChance3 = Random.Range(0, 100);
                    if (itemChance1 >= 0 && itemChance1 <= powerUpChancePercentage)
                    {
                        Instantiate(powerUP, position, Quaternion.identity);
                        //Debug.Log("Power-Up spawned");
                        //Debug.Log(itemChance1);
                    }

                    if (itemChance2 >= 0 && itemChance2 <= bonusChancePercentage)
                    {
                        Instantiate(bonusLife, position, Quaternion.identity);
                        //Debug.Log("Bonus spawned");
                        //Debug.Log(itemChance2);
                    }

                    if (itemChance3 >= 0 && itemChance3 <= hpUpPercentage)
                    {
                        Instantiate(hpUp, position, Quaternion.identity);
                        //Debug.Log("HPUP spawned");
                        //Debug.Log(itemChance3);
                    }
                }
            }
        }
    }
}
