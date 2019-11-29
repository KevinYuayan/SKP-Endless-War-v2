﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class GameController : MonoBehaviour
{
    private int startingLives = 5;

    public bool gameOver;
    public bool restart;
    public bool tutorial = false;
    [Header("Enemies")]
    public int numberOfEnemy1;
    public int numberOfEnemy2;
    public int numberOfEnemy3;
    public int numberOfEnemy4;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public List<GameObject> enemy1s;
    public List<GameObject> enemy2s;
    public List<GameObject> enemy3s;
    public List<GameObject> enemy4s;
    public float spawningDelay;
    [Header("BossEnemy")]
    public GameObject bossEnemy;
    private bool bossSpawned = false;

    [Header("Stage Time Setting")]
    private float time = 0f;
    private float timeCounter;
    public float stageTime;
    private int seconds;

    [Header("ScoreBoard")]
    [SerializeField]
    private int _lives;
    [SerializeField]
    private int _score;
    [SerializeField]
    private int _HP;
    public Text hpLabel;
    public Text livesLabel;
    public Text scoreLabel;
    public Text highScoreLabel;
    public Text timeLabel;
    public Text gameOverLabel;
    public Text restartLabel;
    public Text manualLabel;
    public Text endLabel;

    [Header("Audio Sources")]
    public SoundClip activeSoundClip;
    public AudioSource[] audioSources;

    [Header("UI Control")]
    public GameObject startLabel;
    public GameObject startButton;

    [Header("Bonus")]
    public int bonusSCore = 10000;
    private int bonusStack = 0;
    private bool gotBonus = false;

    [Header("Game Setting")]
    public Storage storage;

    [Header("Scene Settings")]
    public SceneSettings activeSceneSettings;
    public List<SceneSettings> sceneSettings;

    public int HP
    {

        get
        {
            return _HP;
        }
        set
        {
            _HP = value;
            storage.hp = _HP;
            hpLabel.text = "HP: " + _HP.ToString();
        }
    }
    public int Lives
    {
        get
        {
            return _lives;
        }
        set
        {
            _lives = value;
            storage.lives = _lives;
            livesLabel.text = "Lives: " + _lives.ToString();
        }
    }
    public int Score
    {
        get
        {
            return _score;

        }
        set
        {
            _score = value;
            storage.score = _score;

            if(storage.highscore < _score && SceneManager.GetActiveScene().name != "Tutorial")
            {
                storage.highscore = _score;
            }
            if (SceneManager.GetActiveScene().name == "End")
            {
                endLabel.text += _score.ToString();
            }
            else
            {
                scoreLabel.text = "Score : " + _score.ToString();
            }
        
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        highScoreLabel.text = "High Score: " + storage.highscore;

        var query = from settings in sceneSettings
                    where settings.scene == (Scene)Enum.Parse(typeof(Scene), SceneManager.GetActiveScene().name.ToUpper())
                    select settings;

        activeSceneSettings = query.ToList().First();

        startLabel.SetActive(activeSceneSettings.startLabelEnabled);
        manualLabel.enabled = activeSceneSettings.manualLabelEnabled;
        highScoreLabel.enabled = activeSceneSettings.highScoreLabelEnabled;
        scoreLabel.enabled = activeSceneSettings.scoreLabelEnabled;
        livesLabel.enabled = activeSceneSettings.livesLabelEnabled;
        timeLabel.enabled = activeSceneSettings.timeLabelEnabled;
        hpLabel.enabled = activeSceneSettings.hpLabelEnabled;
        endLabel.enabled = activeSceneSettings.endLabelEnabled;
        startButton.SetActive(activeSceneSettings.StartButtonEnabled);

        HP = 100;
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            tutorial = true;
        }
        if (SceneManager.GetActiveScene().name == "Start")
        {
            Lives = startingLives;
            Score = 0;
        }
        else if (SceneManager.GetActiveScene().name == "End")
        {
            restart = true;
            Score = storage.score;
        }
        else
        {
            Lives = storage.lives;
            Score = storage.score;
        }

        if ((activeSoundClip != SoundClip.NONE) && (activeSoundClip != SoundClip.NUM_OF_CLIPS))
        {
            AudioSource activeSoundSource = audioSources[(int)activeSoundClip];
            activeSoundSource.playOnAwake = true;
            activeSoundSource.loop = true;
            activeSoundSource.volume = 0.5f;
            activeSoundSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stageTime - seconds > 0)
        {
                timeLabel.text = "Time: " + (stageTime - seconds);
        }
        else if (stageTime - seconds <= 0)
        {
            timeLabel.text = "Boss spawned!";
        }
        //Setting delay of spawning enemy. 
        time += Time.deltaTime;
        timeCounter += Time.deltaTime;
        seconds = Convert.ToInt32(timeCounter);
        if (timeCounter < stageTime)
        {
            if (time >= spawningDelay && gameOver != true)
            {
                time = 0;
                Spawn();
                //Debug.Log("Enemies spawned");
                //Debug.Log("Time Counter: " + seconds + "Second(s)");
            }
        }

        if (seconds == stageTime && bossSpawned == false
            && SceneManager.GetActiveScene().name != "Start"
            &&SceneManager.GetActiveScene().name != "End"
            && SceneManager.GetActiveScene().name != "Tutorial")
        {
            BossSpawn();
        }

        if (restart == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (SceneManager.GetActiveScene().name == "End")
                {
                    SceneManager.LoadScene("Start");
                }
                else
                {
                    HP = 100;
                    Lives = startingLives;
                    Score = 0;
                    restart = false;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
        //Ading bonus life per a specific score value.
        if(_score >= bonusSCore + (bonusStack * bonusSCore) && gotBonus == false)
        {
            addBonus();
        }
        if(_score > (bonusSCore * bonusStack) && gotBonus == true)
        {
            gotBonus = false;
        }

        //Allow user to skip tutorial
        if(tutorial)
        {
            if (Input.GetKeyDown(KeyCode.K))
               {
                    Lives = startingLives;
                    Score = 0;
                    SceneManager.LoadScene("Main");
               }
        }
    }
    // Spawns small enemies
    void Spawn()
    {
        enemy1s = new List<GameObject>();
        enemy2s = new List<GameObject>();
        enemy3s = new List<GameObject>();
        enemy4s = new List<GameObject>();

        for (int enemy1Num = 0;enemy1Num < numberOfEnemy1; enemy1Num++)
        {
            enemy1s.Add(Instantiate(enemy1));
        }

        for (int enemy2Num = 0; enemy2Num < numberOfEnemy2; enemy2Num++)
        {
            enemy2s.Add(Instantiate(enemy2));
        }

        for(int enemy3Num = 0; enemy3Num < numberOfEnemy3; enemy3Num++)
        {
            enemy3s.Add(Instantiate(enemy3));
        }
        for (int enemy4Num = 0; enemy4Num < numberOfEnemy4; enemy4Num++)
        {
            enemy3s.Add(Instantiate(enemy4));
        }
    }
    void addBonus()
    {
        Lives += 1;
        gotBonus = true;
        bonusStack += 1;
        //Debug.Log("Got bonus by gathering score over bonus score condition.");
    }

    // Called when player's hp hits 0
    public void PlayerDied()
    {
        if (_lives > 1)
        {
            HP = 100;
            Lives -= 1;
            //Debug.Log("Player died");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if( _lives <= 1)
        {
            HP = 100;
            GameOver();
        }
    }

    // Spawns boss when timer hits 0
    void BossSpawn()
    {
        Instantiate(bossEnemy);
        bossSpawned = true;
        //Debug.Log("Boss spawned");
    }

    // Called when player's lives hits 0
    public void GameOver()
    {
        gameOver = true;
        restart = true;
        gameOverLabel.enabled = true;
        restartLabel.enabled = true;
        audioSources[(int)SoundClip.GAME_THEME].Stop();
        audioSources[(int)SoundClip.GAME_OVER].volume = 0.5f;
        audioSources[(int)SoundClip.GAME_OVER].Play();

    }
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void Boss1Defeated()
    {
        SceneManager.LoadScene("Level2");
    }
    public void Boss2Defeated()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Boss3Defeated()
    {
        SceneManager.LoadScene("End");
    }
}
