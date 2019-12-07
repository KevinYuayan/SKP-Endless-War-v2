using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// File Name: PlayerController.cs
/// Author: Hyungseok Philip Lee
/// Last Modified by: Hyungseok Philip Lee
/// Date Last Modified: Dec 06, 2019
/// Description: Controller for the Player prefab
/// Revision History:
/// </summary>

public class TutorialLabelController : MonoBehaviour
{
    public Text enemieSpawnTutorialLabel;
    public SpriteRenderer spawningArrow;
    public Text playerMoveTutorialLabel;
    public SpriteRenderer moveKey1;
    public SpriteRenderer moveKey2;
    public Text playerFireTutorialLabel;
    public SpriteRenderer fireKey1;
    public SpriteRenderer fireKey2;
    public Text timeTutorialLabel;
    public SpriteRenderer timeArrow;
    public Text hpTutorialLabel;
    public SpriteRenderer hpTutorialArrow;
    public Text scoreTutorialLabel;
    public SpriteRenderer scoreTutorialArrow;

    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        enemieSpawnTutorialLabel.enabled = false;
        spawningArrow.enabled = false;
        playerMoveTutorialLabel.enabled = true;
        moveKey1.enabled = true;
        moveKey2.enabled = true;
        playerFireTutorialLabel.enabled = true;
        fireKey1.enabled = true;
        fireKey2.enabled = true;
        timeTutorialLabel.enabled = false;
        timeArrow.enabled = false;
        hpTutorialLabel.enabled = false;
        scoreTutorialLabel.enabled = false;




    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 5f)
        {
            playerMoveTutorialLabel.enabled = false;
            moveKey1.enabled = false;
            moveKey2.enabled = false;
            playerFireTutorialLabel.enabled = false;
            fireKey1.enabled = false;
            fireKey2.enabled = false;
            scoreTutorialLabel.enabled = true;
            scoreTutorialArrow.enabled = true;
        }

         if(time>= 10f)
        {
            scoreTutorialLabel.enabled = false;
            scoreTutorialArrow.enabled = false;
            timeArrow.enabled = true;
            timeTutorialLabel.enabled = true;
        }

        if (time >= 15f)
        {
            timeArrow.enabled = false;
            timeTutorialLabel.enabled = false;
            hpTutorialLabel.enabled = true;
            hpTutorialArrow.enabled = true;
        }

        if(time >= 20f)
        {
            hpTutorialLabel.enabled = false;
            hpTutorialArrow.enabled = false;
            spawningArrow.enabled = true;
            enemieSpawnTutorialLabel.enabled = true;
        }

        if(time >= 25f)
        {
            spawningArrow.enabled = false;
            enemieSpawnTutorialLabel.enabled = false;
        }

    }
}
