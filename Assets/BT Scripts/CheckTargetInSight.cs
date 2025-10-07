using UnityEngine;
using BehaviorTree;

public class CheckTargetInSight : Node
{
    private Transform _transform;
    private float _sightRange;

    public CheckTargetInSight(Transform transform, float sightRange)
    {
        _transform = transform;
        _sightRange = sightRange;
    }

    public override NodeState Evaluate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        float dist = Vector3.Distance(_transform.position, player.transform.position);
        if (dist <= _sightRange)
        {
            parent.SetData("target", player.transform);
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
