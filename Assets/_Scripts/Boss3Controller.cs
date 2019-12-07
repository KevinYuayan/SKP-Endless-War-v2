using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class Boss3Controller : MonoBehaviour
{
    /// <summary>
    /// File Name: Boss3Controller.cs
    /// Author: Seoyoung Kang
    /// Last Modified by: Seoyoung Kang
    /// Date Last Modified: Oct. 18, 2019
    /// Description: Controller of third boss.
    /// Revision History:
    /// </summary>
    [SerializeField]
    private int bossHP;

    [SerializeField]
    public Boundary boundary;

    public float speed;
    private GameController gc;
    private AudioSource explosionSound;

    public GameObject fire;
    public Transform[] fireSpawns;
    public float fireRate;
    private float myTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gco = GameObject.FindWithTag("GameController");
        gc = gco.GetComponent<GameController>();
        explosionSound = gc.audioSources[(int)SoundClip.EXPLOSION];
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
        Vector2 newPosition = new Vector2(0, speed);
        Vector2 currentPosition = transform.position;



        currentPosition -= newPosition;
        transform.position = currentPosition;
    }
    void CheckBounds()
    {
        if (transform.position.y <= boundary.Bottom || transform.position.y >= boundary.Top)
        {
            ChangeDirection();
        }
    }
    void Fire()
    {
        myTime += Time.deltaTime;

        if (myTime > fireRate)
        {
            GameObject fireObj;
            foreach (Transform fireSpawn in fireSpawns)
            {
                fireObj = Instantiate(fire, fireSpawn.position, fireSpawn.rotation);
                fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            }
            myTime = 0.0f;
            //fireSound.volume = 0.3f;
            //fireSound.Play();
        }
    }
    void ChangeDirection()
    {
        speed = speed * -1;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            FireController bulletController = col.GetComponent<FireController>();
            // Checks if bullet is from player
            if (!bulletController.IsEnemyBullet && !bulletController.HasCollided)
            {
                bulletController.HasCollided = true;
                var explosion = PoolManager.GetInstance().GetExplosion();
                explosion.transform.position = col.transform.position;
                Destroy(col.gameObject);
                //explosionSound.volume = 0.3f;
                //explosionSound.Play();
                bossHP -= 1;
                if (bossHP <= 0)
                {
                    Destroy(this.gameObject);
                    gc.Score += 10000;
                    gc.Boss3Defeated();
                }
            }
        }

    }
}