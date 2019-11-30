using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// File Name: SceneSettings.cs
/// Author: Kevin Yuayan
/// Last Modified by: KevinYuayan
/// Date Last Modified: Nov. 5, 2019
/// Description: Scene setting file 
/// Revision History:
/// </summary>

[CreateAssetMenu(fileName = "SceneSettings", menuName = "Scene/Settings")]
[System.Serializable]
public class SceneSettings : ScriptableObject
{
    [Header("Scene Configuration")]
    public Scene scene;

    [Header("Labels")]
    public bool startLabelEnabled;
    public bool manualLabelEnabled;
    public bool highScoreLabelEnabled;
    public bool scoreLabelEnabled;
    public bool livesLabelEnabled;
    public bool timeLabelEnabled;
    public bool hpLabelEnabled;
    public bool endLabelEnabled;
    public bool clearLabelEnabled;
    [Header("Buttons")]
    public bool StartButtonEnabled;

    //[Header("Enemies")]
    //public bool Enemy1;
}
