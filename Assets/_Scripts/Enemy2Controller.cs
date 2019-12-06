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
    private GameController gc;
    private AudioSource explosionSound;

    [SerializeField]
    //Dropping items when the enemy dies
    public GameObject powerUp;
    public GameObject bonusLife;
    public GameObject HPUP;

    private Vector3 position;
    //Item drop chance when enemy dies
    private int itemChance1;
    private int itemChance2;
    private int itemChance3;
    public int powerUpChancePercentage;
    public int bonusChancePercentage;
    public int HPUPChancePercentage;

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
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform.position;
            horizontalSpeed = (transform.position.x - target.x) / 70;
            verticalSpeed = (transform.position.y - target.y) / 70;
        }

        // Rotates the enemy to look at the player's position
        var direction = target - startPosition;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.back);
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

    private void OnTriggerEnter2D(Collider2D col)
    {
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
                Instantiate(powerUp, position, Quaternion.identity);
                //Debug.Log("Power-Up spawned");
                //Debug.Log(itemChance1);
            }

            if (itemChance2 >= 0 && itemChance2 <= bonusChancePercentage)
            {
                Instantiate(bonusLife, position, Quaternion.identity);
                //Debug.Log("Bonus spawned");
                //Debug.Log(itemChance2);
            }

            if (itemChance3 >= 0 && itemChance3 <= HPUPChancePercentage)
            {
                Instantiate(HPUP, position, Quaternion.identity);
                //Debug.Log("HPUP spawned");
                //Debug.Log(itemChance3);
            }
        }
    }
}
