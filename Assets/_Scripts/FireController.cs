using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// File Name: FireController.cs
/// Author: Hyungseok Lee
/// Last Modified by: Hyungseok lee
/// Date Last Modified: Oct. 4, 2019
/// Description: Controller of fire in game.
/// Revision History:
/// </summary>
public class FireController : MonoBehaviour
{
    private bool _isEnemyBullet;
    private Rigidbody2D rBody;
    public float speed;
    // Start is called before the first frame update

    public bool IsEnemyBullet { get; set; }
    public FireController(bool isEnemyBullet = false)
    {
        _isEnemyBullet = isEnemyBullet;
    }
    void Start()
    {
        if (IsEnemyBullet)
        {
            transform.Rotate(0, 0, 180);
            rBody = GetComponent<Rigidbody2D>();
            rBody.velocity = transform.right * speed;
        }
        else
        {
            rBody = GetComponent<Rigidbody2D>();
            rBody.velocity = transform.right * speed;
        }
    }
}
