using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] Transform target;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float fltTowerRange = 15f;

    void Update()
    {
        TargetEnemy();
        AimWeapon();
    }
    
    void TargetEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
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

        target = closestTarget;
    }

    void AimWeapon()
    {
        if (target == null)
        {
            Attack(false);
            return;
        }


        float fltTargetDistance = Vector3.Distance(transform.position, target.position);
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
        var emmissionModule = projectileParticles.emission;
        emmissionModule.enabled = isActive;
        if (target != null)
        {
            weapon.transform.LookAt(target);
        }
    }
}
