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
public class PlayerController : MonoBehaviour
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

    private GameController gc;
    private AudioSource fireSound;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gco = GameObject.FindWithTag("GameController");
        gc = gco.GetComponent<GameController>();
        fireSound = gc.audioSources[(int)SoundClip.PLAYER_FIRE];
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
        if (Input.GetAxis("Vertical") > 0.0f)
        {
            newPosition += new Vector2(0.0f, speed.max);
        }
        if(Input.GetAxis("Vertical") < 0.0f)
        {
             newPosition += new Vector2(0.0f, speed.min);
        }
        if(Input.GetAxis("Horizontal") > 0.0f)
        {
            newPosition += new Vector2(speed.max, 0.0f);
        }
        if(Input.GetAxis("Horizontal") < 0.0f)
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
                    Destroy(col.gameObject);
                    HitbyBullet();
                }
                break;
            case "Enemy1":
            case "Enemy2":
            case "Boss1":
                HitbyCollision();
                break;
        }

        {

        }
    }
    private void HitbyCollision()
    {
        gc.HP -= 10;
        Debug.Log("Hit by collision: " + gc.HP);
        if (gc.HP <= 0)
        {
            Destroy(this.gameObject);
            gc.PlayerDied();
        }
    }
    private void HitbyBullet()
    {
        gc.HP -= 20;
        Debug.Log("Hit by bullet: " + gc.HP);
        if (gc.HP <= 0)
        {
            Destroy(this.gameObject);
            gc.PlayerDied();
        }
    }
}
