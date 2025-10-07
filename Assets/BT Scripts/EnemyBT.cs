using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class EnemyBT : BehaviorTree.Tree
{
    [Header("Patrol")]
    public Transform[] patrolPoints;

    [Header("Ranges")]
    public float sightRange = 12f;
    public float attackRange = 5f;

    [Header("Shooting")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 1f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            // Attack (shoot) sequence
            new Sequence(new List<Node>
            {
                new CheckTargetInAttackRange(transform, attackRange),
                new TaskShoot(transform, firePoint, projectilePrefab, fireRate)
            }),

            // Chase sequence
            new Sequence(new List<Node>
            {
                new CheckTargetInSight(transform, sightRange),
                new TaskChase(transform)
            }),

            // Patrol
            new TaskPatrol(transform, patrolPoints)
        });

        return root;
    }
}
