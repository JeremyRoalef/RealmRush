using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script will be attached to ALL enemies.
 * 
 * This script is responsible for granting and penalizing gold to the player 
 */
public class Enemy : MonoBehaviour
{
    //Serialized fields
    [SerializeField] int goldWhenDestroyed = 5;
    [SerializeField] [Tooltip("How much gold this will remove if it reaches the castle center")] int goldPenalty = 5;

    //Cashe references
    Bank bank;

    //Event Systems
    void Start()
    {
        //set references
        bank = FindObjectOfType<Bank>();
        if (bank == null)
        {
            Debug.Log("There is no bank");
        }
    }

    //Public Methods
    public void RewardGold()
    {
        //DO NOT RUN IF THERE IS NO BANK!!!
        if (bank == null)
        {
            Debug.Log("the is no bank");
            return; 
        }

        //grant gold to the player
        bank.Deposit(goldWhenDestroyed);
    }
    public void RemoveGold()
    {
        //DO NOT RUN IF THERE IS NO BANK!!!
        if (bank == null) 
        {
            Debug.Log("there is no bank");
            return; 
        }

        //Remove gold from player
        bank.Withdraw(goldPenalty);
    }

    //Private Methods

}
