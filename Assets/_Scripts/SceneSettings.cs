using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneSettings", menuName = "Scene/Settings")]
[System.Serializable]
public class SceneSettings : ScriptableObject
{
    public bool StartLabelEnabled;
    public bool StartButtonEnabled;
    public bool scoreLabelEnabled;
    public bool livesLabelEnabled;
    public bool timeLabelEnabled;
    public bool hpLabelEnabled;
    public bool manualLabelEnabled;
}
