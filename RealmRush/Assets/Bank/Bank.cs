using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] int intStartingBalance = 10;
    [SerializeField] int intCurrentBalance;
    public int IntCurrentBalance
    {
        get { return intCurrentBalance; }
    }

    private void Awake()
    {
        intCurrentBalance = intStartingBalance;
    }

    public void Deposit(int amount)
    {
        intCurrentBalance += Mathf.Abs(amount);
    }
    public void Withdraw(int amount)
    {
        intCurrentBalance -= Mathf.Abs(amount);
        if (intCurrentBalance < 0)
        {
            Debug.Log("YOU LOSE");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
