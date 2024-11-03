using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script will be attached to all enemies
 * 
 * This script is responsible for the enemy's health. This includes taking damage from projectiles & other objects as well as increasing the
 * enemy's health when defeated (currently)
 */

//This script requires the enemy component to work
[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    //Serialized fields
    [SerializeField] int maxHp = 5;

    //TODO: enemy health will no longer scale as the object is enabled and disabled. Rather, the enemies will come in waves and health will scale based on variables happening behind the scenes
    [SerializeField] [Tooltip("Add amount to max hp when enemy dies")] int hpIncrement = 1;

    //Cashe references
    Enemy enemy;
    
    //Attributes
    int currentHp = 0;

    //Event Systems
    void OnEnable()
    {
        //Reset hp
        currentHp = maxHp;
    }

    //TODO: Enemy health will not rely on particles in the game to reduce hp. Rather, they will hit triggers that will tell the object how much hp to lose
    void OnParticleCollision(GameObject other)
    {
        //reduce hp
        currentHp -= 1;
        //If out of hp, disable game object, reward gold, & scale max hp
        if (currentHp <= 0)
        {
            gameObject.SetActive(false);
            enemy.RewardGold();
            maxHp += hpIncrement;
        }
    }
    void Start()
    {
        //get components
        enemy = GetComponent<Enemy>();
    }
    //Public Methods

    //Private Methods

}
