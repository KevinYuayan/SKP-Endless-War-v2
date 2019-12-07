using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCleaner : MonoBehaviour
{
    /// <summary>
    /// File Name: ObjectCleaner.cs
    /// Author: Hyungseok Lee
    /// Last Modified by: Hyungseok lee
    /// Date Last Modified: Oct. 8, 2019
    /// Description: Script for removing objects going out of camera
    /// Revision History:
    /// </summary>
    //Removes generated objects that go out of main camera.
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
        }
    }
}
