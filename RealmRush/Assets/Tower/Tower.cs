using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int intTowerCost = 4;

    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindObjectOfType<Bank>();
        if (bank == null ) {return false;}

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
}
