using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    /// <summary>
    /// File Name: ItemController.cs
    /// Author: Hyungseok Lee
    /// Last Modified by: Hyungseok lee
    /// Date Last Modified: Oct. 18, 2019
    /// Description: Controller for Items
    /// Revision History:
    /// </summary>

    [Header("Speed Values")]
    [SerializeField]
    public float horizontalSpeed;
    private GameController gC;
    private PlayerController pC;
    void Start()
    {
        GameObject gCO = GameObject.FindWithTag("GameController");
        gC = gCO.GetComponent<GameController>();
        GameObject pCO = GameObject.FindWithTag("Player");
        pC = pCO.GetComponent<PlayerController>();
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
        if(col.gameObject.tag == "Player" && this.gameObject.tag =="PowerUp" && pC.poweredUp <= 1)
        {
            Destroy(this.gameObject);
            pC.poweredUp += 1;

        }
        if(col.gameObject.tag == "Player" && this.gameObject.tag == "PowerUp" && pC.poweredUp >= 2)
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
            }
            if(gC.HP >= 50)
            {
                gC.HP = 100;
            }
        }
    }
}

