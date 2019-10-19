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
    void Start()
    {
        GameObject gCO = GameObject.FindWithTag("GameController");
        gC = gCO.GetComponent<GameController>();
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
        if(col.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            gC.Lives += 1;
        }
    }
}

