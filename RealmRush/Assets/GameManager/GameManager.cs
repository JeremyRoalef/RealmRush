using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * This script is attached to the GameManager prefab
 * 
 * This script will be responsible for holding the behavior of the game. Some behavior this script will control
 * is the initial setup of waves spawining as well as the current game condition, whether it be where waves
 * are spawning or waves are not spawning and other game conditions that may come later
 */
public class GameManager : MonoBehaviour
{
    [SerializeField] InputAction waveSpawn;
    ObjectPool[] objectPools;
    bool isSpawningWave = false;
    int waveIndex = 0;
    int maxWaveIndex = 0;

    private void Awake()
    {
        objectPools = FindObjectsOfType<ObjectPool>();

        //Get the max number of waves in the game
        foreach(ObjectPool objectPool in objectPools)
        {
            if (objectPool.WaveCount > maxWaveIndex)
            {
                maxWaveIndex = objectPool.WaveCount;
            }
            Debug.Log($"Max wave index = {maxWaveIndex}");
        }
    }
    private void OnEnable()
    {
        waveSpawn.Enable();
    }
    private void OnDisable()
    {
        waveSpawn.Disable();
    }
    private void Update()
    {
        if (waveSpawn.ReadValue<float>() > 0 && !isSpawningWave && waveIndex <= maxWaveIndex)
        {
            isSpawningWave = true;
            //tell all object pools in the scene to spawn the next wave
            foreach (ObjectPool objectPool in objectPools)
            {
                objectPool.StartNextWave(waveIndex);
            }
            waveIndex++;
        }

        if (CanSpawnNextWave())
        {
            isSpawningWave = false;
        }
    }

    public void SetWaveSpawn(ObjectPool objectPool)
    {
        //get object pool in array
        foreach(ObjectPool pool in objectPools)
        {
            if (objectPool == pool)
            {
                //Debug.Log($"Object Pool {pool.name} is no longer spawning");
                pool.IsSpawningWave = false;
                break;
            }
        }
        //Debug.Log($"Can next wave be spawned? {CanSpawnNextWave()}");
    }

    bool CanSpawnNextWave()
    {
        //check if all the waves are done spawning their wave
        for (int i = 0; i < objectPools.Length; i++)
        {
            //If the pool is still spawning wave, return false
            if (objectPools[i].IsSpawningWave)
            {
                return false;
            }
            //Otherwise, if it's the last pool in the list, then no pools are spawning their waves and can return true
            else if ( i == objectPools.Length - 1)
            {
                return true;
            }
        }

        //By default, return false
        return false;
    }
}
