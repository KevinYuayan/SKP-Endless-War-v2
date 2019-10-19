using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    private GameController gc;
    public GameObject bonus;
    private Rigidbody2D rBody;
    public float speed;
    Vector3 position;
    private AudioSource explosionSound;
    private int itemChance;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gco = GameObject.FindWithTag("GameController");
        gc = gco.GetComponent<GameController>();
        rBody = GetComponent<Rigidbody2D>();
        rBody.velocity = transform.right * speed;
        explosionSound = gc.audioSources[(int)SoundClip.EXPLOSION];

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Enemy1")
        {
            Destroy(this.gameObject);
            Destroy(col.gameObject);
            gc.Score += 50;
            explosionSound.volume = 0.3f;
            explosionSound.Play();
        }
        if (col.gameObject.tag == "Enemy2")
        {
            position = this.gameObject.transform.position;
            Destroy(this.gameObject);
            Destroy(col.gameObject);
            gc.Score += 100;
            explosionSound.volume = 0.3f;
            explosionSound.Play();
            itemChance = Random.Range(0, 101);
            if(itemChance >= 0 && itemChance <= 5)
            {
                Instantiate(bonus, position, Quaternion.identity);
            }
}
    }
}
