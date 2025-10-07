using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class TaskPatrol : Node
{
    private Transform _transform;
    private NavMeshAgent _agent;
    private Transform[] _patrolPoints;
    private int _currentIndex = 0;

    public TaskPatrol(Transform transform, Transform[] patrolPoints)
    {
        _transform = transform;
        _agent = transform.GetComponent<NavMeshAgent>();
        _patrolPoints = patrolPoints;
    }

    public override NodeState Evaluate()
    {
        if (_patrolPoints == null || _patrolPoints.Length == 0)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _currentIndex = (_currentIndex + 1) % _patrolPoints.Length;
            _agent.SetDestination(_patrolPoints[_currentIndex].position);
        }

        state = NodeState.RUNNING;
        return state;
    }
}
