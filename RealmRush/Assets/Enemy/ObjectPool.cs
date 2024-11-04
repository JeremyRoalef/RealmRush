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
    //Serialized fields
    [SerializeField] WaveSO[] waves;
    //Property to get number of waves
    public int WaveCount
    {
        get { return waves.Length-1; }
    }

    [SerializeField] [Range(0,50)] int intPoolSize = 5;

    //Cashe references
    GameObject[] enemyPool;
    GameManager gameManager;

    //Attributes
    bool isSpawningWave = false;
    //Property for wave spawning
    public bool IsSpawningWave
    {
        get { return isSpawningWave; }
        set { isSpawningWave = value; }
    }

    //Event Systems
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    //Public Methods
    public void StartNextWave(int index)
    {
        StartCoroutine(SpawnWave(index));
    }

    //Private Methods
    void PopulatePool()
    {
        ////Instantiate enemies to the enemy pool
        //enemyPool = new GameObject[intPoolSize];
        //for (int i = 0; i < enemyPool.Length; i++)
        //{
        //    enemyPool[i] = Instantiate(ram, transform);
        //    enemyPool[i].SetActive(false);
        //}
    }
    void EnableObjectInPool()
    {
        ////enable the first object in the pool that is disabled.
        //foreach (GameObject obj in enemyPool)
        //{
        //    if (obj.activeInHierarchy == false)
        //    {
        //        obj.SetActive(true);
        //        break;
        //    }
        //}
    }
    
    IEnumerator SpawnWave(int index)
    {
        if (index >= waves.Length)
        {
            Debug.Log("No more waves");
            yield break;
        }

        //Spawning wave
        isSpawningWave = true;

        //get the wave's index in the SO
        WaveSO currentWave = waves[index];
        Debug.Log("Spawning wave");

        //Wait the start delay before doing anything
        yield return new WaitForSeconds(currentWave.startDelay);

        //for each enemy in the wave, instantiate and wait between instantiating another one
        foreach (GameObject enemy in currentWave.enemies)
        {
            GameObject tempEnemy = Instantiate(enemy, transform);
            tempEnemy.SetActive(true);
            yield return new WaitForSeconds(currentWave.delayBetweenInstantiation);
        }

        //No longer spawning wave
        gameManager.SetWaveSpawn(this);
    }
}
