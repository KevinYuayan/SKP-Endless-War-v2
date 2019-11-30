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

    public GameObject fireSpawn1;
    public GameObject fireSpawn2;
    public GameObject fireSpawn3;
    public GameObject fireSpawn4;
    public GameObject fireSpawn5;
    public GameObject fireSpawn6;
    public GameObject fireSpawn7;
    public GameObject fireSpawn8;
    public GameObject fireSpawn9;
    public GameObject fireSpawn10;
    public GameObject fire;
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
            fireObj = Instantiate(fire, fireSpawn1.transform.position, fireSpawn1.transform.rotation);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            fireObj = Instantiate(fire, fireSpawn2.transform.position, fireSpawn2.transform.rotation);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            fireObj = Instantiate(fire, fireSpawn3.transform.position, fireSpawn3.transform.rotation);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            fireObj = Instantiate(fire, fireSpawn4.transform.position, fireSpawn4.transform.rotation);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            fireObj = Instantiate(fire, fireSpawn5.transform.position, fireSpawn5.transform.rotation);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            fireObj = Instantiate(fire, fireSpawn6.transform.position, fireSpawn6.transform.rotation);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            fireObj = Instantiate(fire, fireSpawn7.transform.position, fireSpawn7.transform.rotation);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            fireObj = Instantiate(fire, fireSpawn8.transform.position, fireSpawn8.transform.rotation);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            fireObj = Instantiate(fire, fireSpawn9.transform.position, fireSpawn9.transform.rotation);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
            fireObj = Instantiate(fire, fireSpawn10.transform.position, fireSpawn10.transform.rotation);
            fireObj.GetComponent<FireController>().IsEnemyBullet = true;
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
            // Checks if bullet is from player
            if (!col.GetComponent<FireController>().IsEnemyBullet)
            {
                Destroy(col.gameObject);
                explosionSound.volume = 0.3f;
                explosionSound.Play();
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