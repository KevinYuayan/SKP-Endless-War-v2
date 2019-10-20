using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// File Name: CollidableObject.cs
/// Author: Kevin Yuayan
/// Last Modified by: Kevin Yuayan
/// Date Last Modified: Oct. 2, 2019
/// Description: Abstract Class for collidable gameObjects
/// Revision History:
/// </summary>
public abstract class CollidableObject : MonoBehaviour
{
    protected bool _hasCollided;
    public virtual bool HasCollided { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Reset()
    {
        HasCollided = false;
    }
}
