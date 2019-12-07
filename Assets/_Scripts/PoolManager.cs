using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public static Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<Pool> pools;
    
    //private GameController gameController;
   
    //public int numOfObjects;
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
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            container = GameObject.FindGameObjectWithTag(pool.tag + "Container");
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj;
                if (pool.tag == "Explosion")
                {
                    obj = ExplosionFactory.GetInstance().createRandomExplosion();
                }
                else
                {
                    obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                }
                obj.transform.parent = container.transform;
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
        //gameController = gameObject.GetComponent<GameController>();
    }

    public GameObject GetObject(string tag)
    {
        var returnedObject = poolDictionary[tag].Dequeue();
        returnedObject.SetActive(true);
        if(tag == "Explosion")
        {
            returnedObject.GetComponent<Animator>().Play("explosion");
        }
        return returnedObject;
    }

    public void QueueObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
        poolDictionary[gameObject.tag].Enqueue(gameObject);
    }
}
