using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private GameController gameController;

    private GameObject objectToPool;
    private static Queue<GameObject> objectQueue;
    public int numOfObjects;

    public GameObject container;

    private static PoolManager _instance;

    private PoolManager() { }

    public static PoolManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new PoolManager();
        }
        return _instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        container = GameObject.FindGameObjectWithTag("ExplosionContainer");
        gameController = gameObject.GetComponent<GameController>();
        objectToPool = gameController.explosion;

        objectQueue = new Queue<GameObject>();

        for (int i = 0; i < numOfObjects; i++)
        {
            var newExplosion = ExplosionFactory.GetInstance().createRandomExplosion();
            newExplosion.transform.parent = container.transform;
            objectQueue.Enqueue(newExplosion);
        }
    }

    public GameObject GetExplosion()
    {
        var returnedExplosion = objectQueue.Dequeue();
        returnedExplosion.SetActive(true);
        returnedExplosion.GetComponent<Animator>().Play("explosion");
        return returnedExplosion;
    }

    public void ResetExplosion(GameObject explosion)
    {
        explosion.SetActive(false);
        objectQueue.Enqueue(explosion);
    }
}
