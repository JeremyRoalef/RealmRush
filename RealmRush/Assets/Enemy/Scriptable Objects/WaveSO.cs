using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
public class WaveSO : ScriptableObject
{
    //Wave has an array of enemies, start delay, and delay between enemy instantiation
    public GameObject[] enemies;
    [Min(0)] public float startDelay;
    [Min(0)] public float delayBetweenInstantiation;
}
