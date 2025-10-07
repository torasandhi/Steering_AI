using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class TaskShoot : Node
{
    private NavMeshAgent _agent;
    private Transform _firePoint;
    private GameObject _projectilePrefab;
    private float _fireRate;
    private float _fireCooldown;

    public TaskShoot(Transform owner, Transform firePoint, GameObject projectilePrefab, float fireRate = 1f)
    {
        _agent = owner.GetComponent<NavMeshAgent>();
        _firePoint = firePoint;
        _projectilePrefab = projectilePrefab;
        _fireRate = fireRate;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        // stop moving while shooting
        _agent.isStopped = true;
        _fireCooldown -= Time.deltaTime;

        if (_fireCooldown <= 0f)
        {
            Shoot(target);
            _fireCooldown = 1f / _fireRate;
        }

        state = NodeState.RUNNING;
        return state;
    }

    private void Shoot(Transform target)
    {
        GameObject bullet = GameObject.Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);

        // Simple: make bullet fly toward player
        Vector3 dir = (target.position - _firePoint.position).normalized;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = dir * 20f; // adjust projectile speed
    }
}
