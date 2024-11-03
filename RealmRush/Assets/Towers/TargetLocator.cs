using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * This script is attached to any enemy-attacking tower
 * 
 * This script will be used to contorl the behavior of tower targeting. Player should be able to use this to manipulate what their towers
 * will prioritize.
 */
public class TargetLocator : MonoBehaviour
{
    //Serialized fields
    [SerializeField] [Tooltip("The part of the tower that will look at the target position")] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float fltTowerRange = 15f;

    //Cashe references
    Transform target;

    //Event Systems
    void Update()
    {
        TargetEnemy();
        AimWeapon();
    }
    
    //Public Methods

    //Private Methods
    void TargetEnemy()
    {
        //TODO: Change target behavior based on conditions the player wants. EX: target closest or furthst enemy
        //TODO: Add smooth movement between target locking. Current version is super jittery. Can make use of coroutine
        //TODO: stop looking at all enemies in the scene. Rather, look at enemies in the tower's range based on box collider.

        //Get all enemies in the scene
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        //Find the closest target to the ballista
        Transform closestTarget = null;
        float fltMaxDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (targetDistance < fltMaxDistance )
            {
                closestTarget = enemy.transform;
                fltMaxDistance = targetDistance;
            }
        }

        //set the tower's target to the clostest target
        target = closestTarget;
    }
    void AimWeapon()
    {
        //TODO: Add a default state where the tower is not attacking an enemy. Stop staring at the enemies menacingly!

        //If there is no target, do not attack
        if (target == null)
        {
            Attack(false);
            return;
        }

        //calculate distance between target & tower
        float fltTargetDistance = Vector3.Distance(transform.position, target.position);

        //If in range, attack. Otherwise, don't.
        if (fltTargetDistance < fltTowerRange)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }
    void Attack(bool isActive)
    {
        //TODO: change how the tower attacks. Instead of relying on a particle system, use a projectile enemyPool to enable the projectile in the direction you want to fire. This will make life easier later. Trust
        
        //get the emmission module from the particle system attached.
        var emmissionModule = projectileParticles.emission;

        //enable/disable weapon emmission
        emmissionModule.enabled = isActive;

        //if there's a target, look at it
        if (target != null)
        {
            weapon.transform.LookAt(target);
        }
    }
}
