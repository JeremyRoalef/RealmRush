using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] int intStartingBalance = 10;
    [SerializeField] int intCurrentBalance;
    [SerializeField] TextMeshProUGUI textGold;

    public int IntCurrentBalance
    {
        get { return intCurrentBalance; }
    }

    private void Awake()
    {
        intCurrentBalance = intStartingBalance;
        UpdateGoldText();
    }

    public void Deposit(int amount)
    {
        intCurrentBalance += Mathf.Abs(amount);
        UpdateGoldText();
    }
    public void Withdraw(int amount)
    {
        intCurrentBalance -= Mathf.Abs(amount);
        UpdateGoldText();

        if (intCurrentBalance < 0)
        {
            Debug.Log("YOU LOSE");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void UpdateGoldText()
    {
        textGold.text = "Gold: " + intCurrentBalance.ToString();
    }
}
