using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// File Name: Mover.cs
/// Author: Philip Lee
/// Last Modified by: Philip Lee
/// Date Last Modified: Oct. 4, 2019
/// Description: 
/// Revision History:
/// </summary>
public class Mover : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rBody;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        rBody.velocity = transform.right * speed;
    }
}
