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
    //Serialized Fields
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
    bool waveIsOver = true;
    //Property for wave spawning
    public bool WaveIsOver
    {
        get { return waveIsOver; }
        set { waveIsOver = value; }
    }

    //Event Systems
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        //if the wave is not spawning, check if there are any children in the obejct pool (any enemies alive)
        if (!isSpawningWave)
        {
            int numOfChildren = transform.childCount;
            //If there are no children, then it's safe to assume the wave is over.
            if (numOfChildren == 0)
            {
                //Debug.Log($"End of wave for {this.name}");
                waveIsOver = true;
            }
        }
    }
    //Public Methods
    public void StartNextWave(int index)
    {
        StartCoroutine(SpawnWave(index));
    }

    //Private Methods
    IEnumerator SpawnWave(int index)
    {
        if (index >= waves.Length)
        {
            Debug.Log("No more waves");
            yield break;
        }

        //Spawning wave
        isSpawningWave = true;
        waveIsOver = false;

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
        isSpawningWave = false;
    }
}
