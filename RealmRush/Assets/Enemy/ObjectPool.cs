using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject ram;
    [SerializeField] int intPoolSize = 5;
    [SerializeField] float fltInstantiationWaitTime = 1f;
    bool isSpawning = false;

    //Create pool of game object to reduce instantiation happening in the game. May prevent lag
    GameObject[] pool;

    void Start()
    {
        PopulatePool();
    }

    void PopulatePool()
    {
        pool = new GameObject[intPoolSize];
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(ram, transform);
            pool[i].SetActive(false);
        }
    }

    void Update()
    {
        if (!isSpawning)
        {
            StartCoroutine(InstantiateEnemy());
        }
    }

    void EnableObjectInPool()
    {
        foreach (GameObject obj in pool)
        {
            if (obj.activeInHierarchy == false)
            {
                obj.SetActive(true);
                break;
            }
        }
    }

    IEnumerator InstantiateEnemy()
    {
        isSpawning = true;
        yield return new WaitForSeconds(fltInstantiationWaitTime);
        EnableObjectInPool();
        isSpawning = false;
    }
}
