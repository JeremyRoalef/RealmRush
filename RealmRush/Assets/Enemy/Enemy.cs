using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldWhenDestroyed = 5;
    [SerializeField] int goldPenalty = 5;

    Bank bank;

    void Start()
    {
        bank = FindObjectOfType<Bank>();
    }

    public void RewardGold()
    {
        if (bank == null) { return; }
        bank.Deposit(goldWhenDestroyed);
    }
    public void RemoveGold()
    {
        if (bank == null) { return; }
        bank.Withdraw(goldPenalty);
    }
}
