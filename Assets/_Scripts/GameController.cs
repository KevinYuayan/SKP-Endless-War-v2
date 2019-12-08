using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
/// <summary>
/// File Name: GameController.cs
/// Author: Hyungseok Lee
/// Last Modified by: Hyungseok lee
/// Date Last Modified: Nov. 29, 2019
/// Description: GameController for the entire game playing
/// Reference: Tom Tsiliopoulos
/// Revision History:Enemy spawning added, UI setting and sounds controlling added,HP system added,
/// Changing scene by pushing key added, etc...
/// </summary>

public class GameController : MonoBehaviour
{
    private int startingLives = 2;

    public bool gameOver;
    public bool restart;
    public bool tutorial = false;
    [Header("Animations")]
    public GameObject[] explosions;
    [Header("Player")]
    public GameObject player;
    [Header("Enemies")]
    private float enemy1SpawnDelay;
    private float enemy2SpawnDelay;
    private float bomberSpawnDelay;
    private float snakePlaneSpawnDelay;

    private float enemy1SpawnTimer;
    private float enemy2SpawnTimer;
    private float bomberSpawnTimer;
    private float snakePlaneSpawnTimer;

    private bool spawnEnemy1;
    private bool spawnEnemy2;
    private bool spawnBomber;
    private bool spawnSnakePlane;

    public int spawnRateIncreaser = 5;
    private int intSpawnChecker;
    [Header("BossEnemy")]
    public GameObject bossEnemy;
    private bool bossSpawned = false;
    private bool bossDefeated = false;

    [Header("Stage Time Setting")]
    //private float time = 0f;
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
    [Header("UI")]
    public Text hpLabel;
    public Text livesLabel;
    public Text scoreLabel;
    public Text highScoreLabel;
    public Text timeLabel;
    public Text gameOverLabel;
    public Text restartLabel;
    public Text manualLabel;
    public Text endScoreLabel;
    public Text ClearLabel;
    public Text respawnLabel;
    public Text bossSpawningLabel;

    [Header("Audio Sources")]
    public SoundClip activeSoundClip;
    public AudioSource[] audioSources;

    [Header("UI Control")]
    public GameObject startLabel;
    public GameObject startButton;
    public HpBarController hpBarController;

    [Header("Bonus")]
    public int bonusSCore = 10000;
    private int bonusStack = 0;
    private bool gotBonus = false;

    [Header("Game Setting")]
    public Storage storage;

    [Header("Scene Settings")]
    public SceneSettings activeSceneSettings;
    public List<SceneSettings> sceneSettings;

    [Header("Cheat")]
    public PlayerController pC;
    private bool cheatCode = false;

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
            //hpLabel.text = "HP: " + _HP.ToString();
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
            //Score restriction to prevent score farming after stage time is done
            if(timeCounter < stageTime || bossSpawned == true)
            {
                _score = value;
                storage.score = _score;
            }

            if(storage.highscore < _score && SceneManager.GetActiveScene().name != "Tutorial")
            {
                storage.highscore = _score;
            }

            else if (SceneManager.GetActiveScene().name == "End")
            {
                endScoreLabel.text += storage.score;
            }
            scoreLabel.text = "Score : " + storage.score.ToString();
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
        endScoreLabel.enabled = activeSceneSettings.endLabelEnabled;
        ClearLabel.enabled = activeSceneSettings.clearLabelEnabled;

        startButton.SetActive(activeSceneSettings.StartButtonEnabled);

        HP = 100;
        bossDefeated = false;

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            HP = 99999;
            tutorial = true;
        }

        if (SceneManager.GetActiveScene().name == "Start")
        {
            storage.lives = startingLives;
            storage.score = 0;
            storage.poweredUp = 0;
        }

        if (SceneManager.GetActiveScene().name == "End")
        {
            restart = true;
            Score = storage.score;
        }

        else if(SceneManager.GetActiveScene().name =="Tutorial"
            || SceneManager.GetActiveScene().name == "Main"
            || SceneManager.GetActiveScene().name == "Level2"
            || SceneManager.GetActiveScene().name == "Level3")
        {
            Lives = storage.lives;
            Score = storage.score;
            Instantiate(player);
            GameObject pCO = GameObject.FindWithTag("Player");
            pC = pCO.GetComponent<PlayerController>();
            cheatCode = true;
        }

        if ((activeSoundClip != SoundClip.NONE) && (activeSoundClip != SoundClip.NUM_OF_CLIPS))
        {
            AudioSource activeSoundSource = audioSources[(int)activeSoundClip];
            activeSoundSource.playOnAwake = true;
            activeSoundSource.loop = true;
            activeSoundSource.volume = 0.3f;
            activeSoundSource.Play();
        }
        // Sets the initial spawndelay for each enemy
        if(activeSceneSettings.numOfEnemy1 > 0)
        {
            spawnEnemy1 = true;
            enemy1SpawnDelay = 10f / activeSceneSettings.numOfEnemy1;
        }
        if (activeSceneSettings.numOfEnemy2 > 0)
        {
            spawnEnemy2 = true;
            enemy2SpawnDelay = 10f / activeSceneSettings.numOfEnemy2;
        }
        if (activeSceneSettings.numOfBomber > 0)
        {
            spawnBomber = true;
            bomberSpawnDelay = 10f / activeSceneSettings.numOfBomber;
        }
        if (activeSceneSettings.numOfSnakePlane > 0)
        {
            spawnSnakePlane = true;
            snakePlaneSpawnDelay = 10f / activeSceneSettings.numOfSnakePlane;
        }
        intSpawnChecker = spawnRateIncreaser;
    }
    // Increases Spawn Rate of enemies every 5 seconds(spawnRateIncreaser default)
    private void IncreaseSpawnRate()
    {
        if (seconds >= intSpawnChecker)
        {
            intSpawnChecker += spawnRateIncreaser;
            // Max Spawn Rate check
            // Enemy1 is 20 planes / 10 sec
            if (enemy1SpawnDelay > 0.5f)
            {
                enemy1SpawnDelay *= 0.9f;
            }
            // Enemy 2 is about 14 plane / 10 sec
            if (enemy2SpawnDelay > 0.7f)
            {
                enemy2SpawnDelay *= 0.9f;
            }
            // Bomber is about 7 planes / 10 sec
            if (bomberSpawnDelay > 1.5f)
            {
                bomberSpawnDelay *= 0.9f;
            }
            // SnakePlane is about 7 planes / 10 sec
            if (snakePlaneSpawnDelay > 1.5f)
            {
                snakePlaneSpawnDelay *= 0.9f;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (stageTime - seconds > 0 && !(tutorial))
        {
            timeLabel.text = "Time: " + (stageTime - seconds);
        }
        else if (stageTime - seconds <= 0 && bossSpawned == false)
        {
            timeLabel.text = "Push 'G' key to defat boss!";
        }

        timeCounter += Time.deltaTime;
        seconds = Convert.ToInt32(timeCounter);

        Spawn();
        IncreaseSpawnRate();

        if (seconds > stageTime 
            && bossSpawned == false
            && SceneManager.GetActiveScene().name != "Start"
            &&SceneManager.GetActiveScene().name != "End"
            && SceneManager.GetActiveScene().name != "Tutorial"
            &&Input.GetKeyDown(KeyCode.G))
        {
            bossSpawned = true;
            bossSpawningLabel.enabled = true;
            StartCoroutine(BossSpawner());
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
                    storage.poweredUp = 0;
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
                    storage.poweredUp = 0;
                    pC.poweredUp = 0;
                    SceneManager.LoadScene("Main");
               }
        }

        //Allow user to go to the next level
        if (Input.GetKeyDown(KeyCode.G)
            && SceneManager.GetActiveScene().name == "Main"
            && bossDefeated == true)
        {
            SceneManager.LoadScene("Level2");
        }

        //CheatKey
        if(cheatCode == true)
        {
            if(Input.GetKeyDown(KeyCode.S)
                && Input.GetKeyDown(KeyCode.K))
            {
                GameObject hpO = GameObject.FindWithTag("HpStatus");
                hpBarController = hpO.GetComponent<HpBarController>();
                _HP += 99999;
                hpBarController.health = _HP / 100;
                pC.poweredUp = 2;
                storage.poweredUp = 2;
                BossSpawn();
                //Debug.Log("Cheatcode executed");
            }

            if(Input.GetKeyDown(KeyCode.K) &&
                Input.GetKeyDown(KeyCode.P))
            {
                if(SceneManager.GetActiveScene().name == "Main")
                {
                    SceneManager.LoadScene("Level2");
                }
                else if(SceneManager.GetActiveScene().name == "Level2")
                {
                    SceneManager.LoadScene("Level3");
                }

                else if(SceneManager.GetActiveScene().name == "Level3")
                {
                    SceneManager.LoadScene("End");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G)
            && SceneManager.GetActiveScene().name == "Level2"
            && bossDefeated == true)
        {
            SceneManager.LoadScene("Level3");
        }
    }
    // Spawns small enemies
    void Spawn()
    {
        if(!gameOver && !bossDefeated)
        {
            enemy1SpawnTimer += Time.deltaTime;
            enemy2SpawnTimer += Time.deltaTime;
            bomberSpawnTimer += Time.deltaTime;
            snakePlaneSpawnTimer += Time.deltaTime;

            if (spawnEnemy1 && enemy1SpawnTimer >= enemy1SpawnDelay)
            {
                var enemy = PoolManager.GetInstance().GetObject("Enemy1");
                enemy1SpawnTimer = 0;
            }
            if (spawnEnemy2 && enemy2SpawnTimer >= enemy2SpawnDelay)
            {
                var enemy = PoolManager.GetInstance().GetObject("Enemy2");
                enemy2SpawnTimer = 0;
            }
            if (spawnBomber && bomberSpawnTimer >= bomberSpawnDelay)
            {
                var enemy = PoolManager.GetInstance().GetObject("Bomber");
                bomberSpawnTimer = 0;
            }
            if (spawnSnakePlane && snakePlaneSpawnTimer >= snakePlaneSpawnDelay)
            {
                var enemy = PoolManager.GetInstance().GetObject("SnakePlane");
                snakePlaneSpawnTimer = 0;
            }
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
        if (_lives > 0)
        {
            HP = 100;
            Lives -= 1;
            //storage.poweredUp = 0;
            //Debug.Log("Player died");
            respawnLabel.enabled = true;
            StartCoroutine(RespawnPlayer());
        }
        else if( _lives <= 0)
        {
            HP = 100;
            GameOver();
        }
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(2);
        respawnLabel.enabled = false;
        Instantiate(player);
    }

    private IEnumerator BossSpawner()
    {
        yield return new WaitForSeconds(2);
        bossSpawningLabel.enabled = false;
        BossSpawn();
    }

    // Spawns boss when timer hits 0
    void BossSpawn()
    {
        bossSpawningLabel.enabled = false;
        timeLabel.text = "Boss spawned!";
        Instantiate(bossEnemy);
        bossSpawned = true;
        //Debug.Log(bossSpawned);
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
        ClearLabel.enabled = true;
        bossDefeated = true;
    }
    public void Boss2Defeated()
    {
        ClearLabel.enabled = true;
        bossDefeated = true;
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("Level3");
        }
    }

    public void Boss3Defeated()
    {
        SceneManager.LoadScene("End");
    }
}
