using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// File Name: Storage.cs
/// Author: Hyungseok Lee
/// Last Modified by: Hyungseok lee
/// Date Last Modified: Nov. 29, 2019
/// Reference: Tom Tsiliopoulos
/// Description: Scriptable object of player status value.
/// Revision History:
/// </summary>
/// 
[CreateAssetMenu(fileName = "Storage", menuName = "Game/Storage")]
[System.Serializable]
public class Storage : ScriptableObject
{
    public int hp;
    public int lives;
    public int score;
    public int highscore;
    public int poweredUp;
}
