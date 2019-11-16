using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Storage", menuName = "Game/Storage")]
[System.Serializable]
public class Storage : ScriptableObject
{
    public int lives;
    public int score;
    public int highscore;
}
