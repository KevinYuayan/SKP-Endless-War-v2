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
}
