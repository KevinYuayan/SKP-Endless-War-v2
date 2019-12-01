using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    /// <summary>
    /// File Name: ItemController.cs
    /// Author: Hyungseok Lee
    /// Last Modified by: Hyungseok lee
    /// Date Last Modified: Nov. 29, 2019
    /// Description: Controller for Items
    /// Revision History:Hp-Up Item added, hp bar controlling by getting hp-up item added.
    /// </summary>

    [Header("Speed Values")]
    [SerializeField]
    public float horizontalSpeed;
    [Header("Storage")]
    public Storage storage;
    private GameController gC;
    private PlayerController pC;
    //HpBarController reference
    private HpBarController hpBC;
    void Start()
    {
        GameObject gCO = GameObject.FindWithTag("GameController");
        gC = gCO.GetComponent<GameController>();

        GameObject pCO = GameObject.FindWithTag("Player");
        pC = pCO.GetComponent<PlayerController>();

        GameObject hpBCO = GameObject.FindWithTag("HpStatus");
        hpBC = hpBCO.GetComponent<HpBarController>();
        //Debug.Log("Power up level: " + pC.poweredUp);

    }
    void Update()
    {
        Move();
    }
    void Move()
    {
        Vector2 newPosition = new Vector2(horizontalSpeed, 0);
        Vector2 currentPosition = transform.position;

        currentPosition -= newPosition;
        transform.position = currentPosition;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" && this.gameObject.tag == "BonusLife")
        {
            Destroy(this.gameObject);
            gC.Lives += 1;
        }
        if(col.gameObject.tag == "Player" && this.gameObject.tag =="PowerUp" && storage.poweredUp <= 1)
        {
            Destroy(this.gameObject);
            pC.poweredUp += 1;

        }
        if(col.gameObject.tag == "Player" && this.gameObject.tag == "PowerUp" && storage.poweredUp >= 2)
        {
            Destroy(this.gameObject);
            gC.Score += 100;
        }

        if(col.gameObject.tag == "Player" && this.gameObject.tag == "HPUP")
        {
            Destroy(this.gameObject);
            if(gC.HP <= 50)
            {
                gC.HP += 50;
                hpBC.health += 0.5f;
                //Debug.Log("Hp 50% retored");
            }
            else if(gC.HP > 50)
            {
                gC.HP = 100;
                hpBC.health = 1.0f;
                //Debug.Log("Hp is full");
            }
        }
    }
}

