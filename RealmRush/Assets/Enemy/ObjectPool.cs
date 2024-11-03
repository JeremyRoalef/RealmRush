using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is attached to the object pool prefab
 * 
 * This script will be responsible for spawning in waves of customized enemies for a given path
 */
public class ObjectPool : MonoBehaviour
{
    //TODO: Object pool will no longer rely on one type of enemy to spawn enemies, but rather will accept an array of enemies that will release
    //TODO: Add an initial wait time before letting the objects in the pool go.

    //Serialized fields
    [SerializeField] GameObject ram;
    [SerializeField] [Range(0,50)] int intPoolSize = 5;
    [SerializeField] [Range(0.1f, 180f)] float fltInstantiationWaitTime = 1f;

    //Cashe references
    //TODO: change the enemy pool to a serialized field. The pool of enemies will be determined within unity & this script's job will simply be to instantiate & enable the enemies in the game
    GameObject[] enemyPool;

    //Attributes
    bool isSpawning = false;

    //Event Systems
    void Start()
    {
        //Add enemies to the pool
        PopulatePool();
    }
    void Update()
    {
        //if no enemies are spawning, spawn enemies
        if (!isSpawning)
        {
            StartCoroutine(InstantiateEnemy());
        }
    }
    //Public Methods

    //Private Methods
    void PopulatePool()
    {
        //Instantiate enemies to the enemy pool
        enemyPool = new GameObject[intPoolSize];
        for (int i = 0; i < enemyPool.Length; i++)
        {
            enemyPool[i] = Instantiate(ram, transform);
            enemyPool[i].SetActive(false);
        }
    }
    void EnableObjectInPool()
    {
        //enable the first object in the pool that is disabled.
        foreach (GameObject obj in enemyPool)
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
        //spawning enemy
        isSpawning = true;

        //wait to spawn next enemy
        yield return new WaitForSeconds(fltInstantiationWaitTime);
        EnableObjectInPool();

        //no longer spawning enemy
        isSpawning = false;
    }
}
