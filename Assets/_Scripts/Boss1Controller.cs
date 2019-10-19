using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    [SerializeField]
    private int bosSHP;

    private GameController gc;
    private AudioSource explosionSound;


    // Start is called before the first frame update
    void Start()
    {
        GameObject gco = GameObject.FindWithTag("GameController");
        gc = gco.GetComponent<GameController>();
        explosionSound = gc.audioSources[(int)SoundClip.EXPLOSION];
=======
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            Destroy(col.gameObject);
            explosionSound.volume = 0.3f;
            explosionSound.Play();
            if (bosSHP != 0)
            {
                bosSHP -= 1;
            }
            if (bosSHP <= 0)
            {
                Destroy(this.gameObject);
                gc.Score += 1000;
            }
        }

=======
        
    }
}
