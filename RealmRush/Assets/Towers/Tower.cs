using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: this script should be attached to a new game object that allows the player to choose between existing towers. Remove this script from the ballista prefab.

/*
 * This script is currently attached to the ballista prevab. This script will be responsible for creating and placing towers on tiles
 */

public class Tower : MonoBehaviour
{
    //Serialized fields
    [SerializeField] int intTowerCost = 4;
    [SerializeField] float fltBuildDelay;

    //Event Systems
    private void Start()
    {
        //TODO: add a UI to better visually indicate that this tower was created & is being built
        StartCoroutine(Build());
    }

    //Public Methods
    public bool CreateTower(Tower tower, Vector3 position)
    {
        //Find the bank in the game
        Bank bank = FindObjectOfType<Bank>();

        //DO NOT PROCEED IF THERE IS NO BANK OBJECT
        if (bank == null )
        {
            Debug.Log("No bank found");
            return false;
        }

        //Create tower if the player can afford the cost
        if (bank.IntCurrentBalance >= intTowerCost)
        {
            Instantiate(tower, position, Quaternion.identity);
            bank.Withdraw(intTowerCost);
            return true;
        }
        else
        {
            return false;
        }
    }

    //Private Methods
    IEnumerator Build()
    {
        //Disable all the tower components
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        //Enable all tower components after a delay
        foreach (Transform child in transform)
        {
            yield return new WaitForSeconds(fltBuildDelay);
            child.gameObject.SetActive(true);

        }
    }
}
