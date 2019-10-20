using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
