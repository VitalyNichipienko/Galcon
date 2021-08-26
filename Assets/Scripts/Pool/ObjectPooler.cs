using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region Nested Class

    [Serializable]
    public class Pool
    {
        public enum ObjectType
        {
            Circle,
            Line,
            PlayerShip,
            EnemyShip
        }

        public ObjectType type;
        public GameObject prefab;
        public int startSize;

        [HideInInspector] public Transform root;
    }

    #endregion



    #region Fields

    public static ObjectPooler Instance;

    public Pool[] pools = new Pool[Enum.GetNames(typeof(Pool.ObjectType)).Length];
    public Dictionary<Pool.ObjectType, Queue<GameObject>> poolDictionary;

    #endregion



    #region Methods

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        CreateObjectPools();
    }


    private void CreateObjectPools()
    {
        poolDictionary = new Dictionary<Pool.ObjectType, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            GameObject rootObj = new GameObject("Root " + pool.type);
            pool.root = rootObj.transform;

            for (int i = 0; i < pool.startSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(pool.root);                
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.type, objectPool);
        }
    }


    public GameObject SpawnFromPool(Pool.ObjectType tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist");
            return null;
        }

        GameObject objectToSpawn;

        if (poolDictionary[tag].Count == 0)
        {
            objectToSpawn = Instantiate(pools[(int)tag].prefab);
            objectToSpawn.transform.SetParent(pools[(int)tag].root);
        }
        else
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
            objectToSpawn.SetActive(true);           
        }

        return objectToSpawn;
    }


    public void ReturnToPool(Pool.ObjectType tag, GameObject objectToReturn) 
    {
        objectToReturn.SetActive(false);
        objectToReturn.transform.SetParent(pools[(int)tag].root);
        poolDictionary[tag].Enqueue(objectToReturn);
    }

    #endregion
}
