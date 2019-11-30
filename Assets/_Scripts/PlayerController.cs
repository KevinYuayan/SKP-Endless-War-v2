using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

/// <summary>
/// File Name: PlayerController.cs
/// Author: Philip Lee
/// Last Modified by: Philip Lee
/// Date Last Modified: Oct. 4, 2019
/// Description: Controller for the Player prefab
/// Revision History:
/// </summary>
public class PlayerController : CollidableObject
{
    public Speed speed;
    public Boundary boundary;
    [Header("Attack Settings")]
    public GameObject fire;
    public GameObject fireSpawn1;
    public GameObject fireSpawn2;
    public GameObject fireSpawn3;
    public float fireRate = 0.5f;
    private float myTime = 0.0f;
    public int poweredUp = 0;

    // private instance variables
    private int _invincibilityTime = 1;
    private float _invincibleOpacity = 0.5f;
    private GameController gc;
    private AudioSource fireSound;

    //HpBarController reference
    private HpBarController hpBC;

    // Properties
    public override bool HasCollided
    {
        get
        {
            return _hasCollided;
        }
        set
        {
            _hasCollided = value;
            if (value == true)
            {
                StartCoroutine(ITime());
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject gco = GameObject.FindWithTag("GameController");
        GameObject hpBCO = GameObject.FindWithTag("HpStatus");

        gc = gco.GetComponent<GameController>();
        hpBC = hpBCO.GetComponent<HpBarController>();

        fireSound = gc.audioSources[(int)SoundClip.PLAYER_FIRE];
    }
    // Method called when player is hit and becomes invincible
    private IEnumerator ITime()
    {
        this.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, _invincibleOpacity);
        //Debug.Log("Invincible");
        yield return new WaitForSeconds(_invincibilityTime);
        //Debug.Log("before Invincible turned off");
        HasCollided = false;
        this.gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckBounds();
        Fire();
    }
    void Move()
    {
        Vector2 newPosition = transform.position;
        if (Input.GetAxisRaw("Vertical") > 0.0f)
        {
            newPosition += new Vector2(0.0f, speed.max);
        }
        if(Input.GetAxisRaw("Vertical") < 0.0f)
        {
             newPosition += new Vector2(0.0f, speed.min);
        }
        if(Input.GetAxisRaw("Horizontal") > 0.0f)
        {
            newPosition += new Vector2(speed.max, 0.0f);
        }
        if(Input.GetAxisRaw("Horizontal") < 0.0f)
        {
            newPosition += new Vector2(speed.min, 0.0f);
        }
        transform.position = newPosition;
    }

    public void CheckBounds()
    {
        if (transform.position.x < boundary.Left)
        {
            transform.position = new Vector2(boundary.Left, transform.position.y);
        }
        //Check right boundary.
        if (transform.position.x > boundary.Right)
        {
            transform.position = new Vector2(boundary.Right, transform.position.y);
        }
        if(transform.position.y > boundary.Top)
        {
            transform.position = new Vector3(transform.position.x,boundary.Top);
        }
        if(transform.position.y < boundary.Bottom)
        {
            transform.position = new Vector3(transform.position.x, boundary.Bottom);
        }
    }
    // Fires bullets based on player's power
    void Fire()
    {
        myTime += Time.deltaTime;

        if (Input.GetButton("Fire1") && myTime > fireRate)
        {
            Instantiate(fire, fireSpawn1.transform.position, fireSpawn1.transform.rotation);
            if(poweredUp >= 1)
            {
                Instantiate(fire, fireSpawn2.transform.position, fireSpawn2.transform.rotation);
            }
            if(poweredUp == 2)
            {
                Instantiate(fire, fireSpawn3.transform.position, fireSpawn3.transform.rotation);
            }
            myTime = 0.0f;
            fireSound.volume = 0.3f;
            fireSound.Play();
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        switch(col.gameObject.tag)
        {
            case "Bullet":
                if (col.gameObject.GetComponent<FireController>().IsEnemyBullet)
                {
                    if (!HasCollided)
                    {
                        Destroy(col.gameObject);
                        HitbyBullet();
                        HasCollided = true;
                    }
                }
                break;
            case "Enemy1":
            case "Enemy2":
            case "Bomber":
            case "Boss1":
            case "Boss2":
            case "Boss3":
                if(!HasCollided)
                {
                    HitbyCollision();
                    HasCollided = true;
                }
                break;
        }

        {

        }
    }
    // Methods that handles the player hit by an enemy object
    private void HitbyCollision()
    {
        gc.HP -= 10;
        hpBC.SetDamage(10);
        //Debug.Log("Hit by collision: " + gc.HP);
        if (gc.HP <= 0)
        {
            Destroy(this.gameObject);
            gc.PlayerDied();
        }
    }
    private void HitbyBullet()
    {
        gc.HP -= 20;
        hpBC.SetDamage(20);
        //Debug.Log("Hit by bullet: " + gc.HP);
        if (gc.HP <= 0)
        {
            Destroy(this.gameObject);
            gc.PlayerDied();
        }
    }
}
