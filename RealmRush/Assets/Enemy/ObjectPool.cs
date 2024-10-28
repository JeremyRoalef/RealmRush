using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject ram;
    [SerializeField] float fltInstantiationWaitTime = 1f;
    bool isSpawning = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpawning)
        {
            StartCoroutine(InstantiateEnemy());
        }
    }


    IEnumerator InstantiateEnemy()
    {
        isSpawning = true;
        yield return new WaitForSeconds(fltInstantiationWaitTime);
        Instantiate(ram);
        isSpawning = false;
    }
}
