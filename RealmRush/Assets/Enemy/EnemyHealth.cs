using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHp = 5;

    [Tooltip("Add amount to max hp when enemy dies")]
    [SerializeField] int hpIncrement = 1;

    Enemy enemy;
    int currentHp = 0;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        currentHp = maxHp;
    }

    private void OnParticleCollision(GameObject other)
    {
        currentHp -= 1;
        Debug.Log(currentHp);
        if (currentHp <= 0)
        {
            gameObject.SetActive(false);
            enemy.RewardGold();
            maxHp += hpIncrement;
        }
    }
}
