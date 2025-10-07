using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

public class TaskChase : Node
{
    private NavMeshAgent _agent;

    public TaskChase(Transform transform)
    {
        _agent = transform.GetComponent<NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        _agent.isStopped = false; // make sure movement is on
        _agent.SetDestination(target.position);

        state = NodeState.RUNNING;
        return state;
    }
}
