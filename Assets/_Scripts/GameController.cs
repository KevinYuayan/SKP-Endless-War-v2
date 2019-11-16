using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    public bool gameOver;
    public bool restart;
    [Header("Enemies")]
    public int numberOfEnemy1;
    public int numberOfEnemy2;
    public int numberOfEnemy3;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public List<GameObject> enemy1s;
    public List<GameObject> enemy2s;
    public List<GameObject> enemy3s;
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
    public Text highscoreLabel;
    public Text timeLabel;
    public Text gameOverLabel;
    public Text restartLabel;

    [Header("Audio Sources")]
    public SoundClip activeSoundClip;
    public AudioSource[] audioSources;

    [Header("UI Control")]
    public GameObject StartLabel;
    public GameObject StartButton;

    [Header("Bonus")]
    public int bonusSCore = 10000;
    private int bonusStack = 0;
    private bool gotBonus = false;

    [Header("GameSetting")]
    public Storage storage;

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

            if(storage.highscore < _score)
            {
                storage.highscore = _score;
            }

            scoreLabel.text = "Score : " + _score.ToString();
        }
    }

    public
    // Start is called before the first frame update
    void Start()
    {
        highscoreLabel.text = "High Score: " + storage.highscore;

        if ((activeSoundClip != SoundClip.NONE) && (activeSoundClip != SoundClip.NUM_OF_CLIPS))
        {
            AudioSource activeSoundSource = audioSources[(int)activeSoundClip];
            activeSoundSource.playOnAwake = true;
            activeSoundSource.loop = true;
            activeSoundSource.volume = 0.5f;
            activeSoundSource.Play();
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Start":
                scoreLabel.enabled = false;
                livesLabel.enabled = false;
                timeLabel.enabled = false;
                hpLabel.enabled = false;
                HP = 100;
                Lives = 5;
                Score = 0;
                break;
            case "Main":
                StartLabel.SetActive(false);
                StartButton.SetActive(false);
                Lives = storage.lives;
                Score = storage.score;
                HP = 100;
                break;
            case "End":
                scoreLabel.enabled = false;
                livesLabel.enabled = false;
                timeLabel.enabled = false;
                StartLabel.SetActive(false);
                StartButton.SetActive(false);
                break;
            case "Level2":
                StartLabel.SetActive(false);
                StartButton.SetActive(false);
                Lives = storage.lives;
                Score = storage.score;
                HP = 100;
                break;

            case "Level3":
                StartLabel.SetActive(false);
                StartButton.SetActive(false);
                Lives = storage.lives;
                Score = storage.score;
                HP = 100;
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreLabel.text = "Score: " + _score.ToString();
        livesLabel.text = "Lives: " + _lives.ToString();
        hpLabel.text = "HP: " + _HP.ToString();

        if (stageTime - seconds > 0)
        {
            timeLabel.text = "Time: " + (stageTime - seconds);
        }
        else if (stageTime - seconds <= 0)
        {
            timeLabel.text = "Boss spawned!";
        }
        //Setting delay of spawning enemy. time % nf means n 
        time += Time.deltaTime;
        timeCounter += Time.deltaTime;
        seconds = Convert.ToInt32(timeCounter);
        if (timeCounter < stageTime)
        {
            if (time >= spawningDelay && gameOver != true)
            {

                time = time % 1f;
                Spawn();
                Debug.Log("Enemies spawned");
                Debug.Log("Time Counter: " + seconds + "Second(s)");
            }
        }

        if (seconds == stageTime && bossSpawned == false
            && SceneManager.GetActiveScene().name != "Start")
        {
            BossSpawn();
        }

        if (restart == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                HP = 100;
                Lives = 5;
                Score = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
    }
    void Spawn()
    {
        enemy1s = new List<GameObject>();
        enemy2s = new List<GameObject>();
        enemy3s = new List<GameObject>();

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
    }
    void addBonus()
    {
        _lives += 1;
        gotBonus = true;
        bonusStack += 1;
        Debug.Log("Got bonus by gathering score over bonus score condition.");
    }

    public void PlayerDied()
    {
        if (_lives > 0)
        {
            HP = 1000;
            Lives -= 1;
            Debug.Log("Player died");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if( _lives <= 0)
        {
            HP = 1000;
            GameOver();
        }
    }

    void BossSpawn()
    {
        Instantiate(bossEnemy);
        bossSpawned = true;
        Debug.Log("Boss spawned");
    }
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
        SceneManager.LoadScene("Main");
    }
    public void Boss1Defeated()
    {
        SceneManager.LoadScene("Level2");
    }
    public void Boss2Defeated()
    {
        SceneManager.LoadScene("Level3");
    }
}
