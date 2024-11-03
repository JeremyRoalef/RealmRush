using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This script is attached to the bank gameObject
 * 
 * This script will be responsible for holding the player's currency. 
 */

public class Bank : MonoBehaviour
{
    //Serialized fields
    [SerializeField] [Tooltip("The player's starting gold for the scene")] int intStartingBalance = 10;
    [SerializeField] [Tooltip("The gold text in the canvas overlay")] TextMeshProUGUI textGold;

    //Attributes
    int intCurrentBalance;
    //Propety to get the balance in other scripts & set the gold text when it is changed
    public int IntCurrentBalance
    {
        get { return intCurrentBalance; }
        set 
        {
            intCurrentBalance = value;
            UpdateGoldText(); 
        }
    }

    //Event Systems
    private void Awake()
    {
        //Set current balance to starting balance & display the current gold
        IntCurrentBalance = intStartingBalance;
    }

    //Public Methods
    public void Deposit(int amount)
    {
        //Add amount to gold
        IntCurrentBalance += Mathf.Abs(amount);
    }
    public void Withdraw(int amount)
    {
        //Withdraw amount from gold
        IntCurrentBalance -= Mathf.Abs(amount);

        //TODO: Change the losing condition of the game. Lose condition should be based on castle center health falling to or below 0, not from running out of gold.
        
        //If negative balance, reload game.
        if (intCurrentBalance < 0)
        {
            Debug.Log("YOU LOSE");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //Private Methods
    void UpdateGoldText()
    {
        textGold.text = "Gold: " + intCurrentBalance.ToString();
    }
}
