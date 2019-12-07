using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionFactory : MonoBehaviour
{
    private static GameController gameController;
    private static GameObject[] explosions;

    private static ExplosionFactory _instance;

    private ExplosionFactory() { }

    public static ExplosionFactory GetInstance()
    {
        if(_instance == null)
        {
            _instance = new ExplosionFactory();
        }
        return _instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = gameObject.GetComponent<GameController>();
        explosions = gameController.explosions;
    }

    public GameObject createRandomExplosion()
    {
        var randomTypeOfExplosion = Random.Range(0, 4);
        var randomTransparency = new Color(1, 1, 1, Random.Range(0.8f, 1.0f));
        var randomRotation = Random.Range(0.0f, 40.0f);
        var randomScale = Random.Range(0.9f, 1.1f);
        var newExplosion = Instantiate(explosions[randomTypeOfExplosion], Vector2.zero, Quaternion.identity);
        newExplosion.GetComponent<SpriteRenderer>().material.color = randomTransparency;
        newExplosion.transform.Rotate(new Vector3(0f, 0f, randomRotation));
        newExplosion.transform.localScale = new Vector3(randomScale, randomScale, 1);
        newExplosion.SetActive(false);

        return newExplosion;
    }
}
