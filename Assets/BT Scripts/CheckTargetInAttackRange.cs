using UnityEngine;
using BehaviorTree;

public class CheckTargetInAttackRange : Node
{
    private Transform _transform;
    private float _attackRange;

    public CheckTargetInAttackRange(Transform transform, float attackRange)
    {
        _transform = transform;
        _attackRange = attackRange;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if (target == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        float dist = Vector3.Distance(_transform.position, target.position);
        if (dist <= _attackRange)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
