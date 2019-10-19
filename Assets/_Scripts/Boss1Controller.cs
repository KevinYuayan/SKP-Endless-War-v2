using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss1Controller : MonoBehaviour
{
    [SerializeField]
    private int bossHP;

    private GameController gc;
    private AudioSource explosionSound;


    // Start is called before the first frame update
    void Start()
    {
        GameObject gco = GameObject.FindWithTag("GameController");
        gc = gco.GetComponent<GameController>();
        explosionSound = gc.audioSources[(int)SoundClip.EXPLOSION];
    }
        // Update is called once per frame
        void Update()
        {
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Bullet")
            {
                Destroy(col.gameObject);
                explosionSound.volume = 0.3f;
                explosionSound.Play();
                if (bossHP > 0)
                {
                    bossHP -= 1;
                }
                if (bossHP <= 0)
                {
                    Destroy(this.gameObject);
                    gc.Score += 1000;
                SceneManager.LoadScene("Level2");
                }
            }

        }
 }

