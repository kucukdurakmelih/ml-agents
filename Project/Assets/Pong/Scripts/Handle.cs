using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class Handle : Agent
{
    [field: SerializeField] private float Speed { get; set; } = 5f;
    [field: SerializeField] private Vector3 _startPos;
    private Rigidbody _rb;
    public Team team;


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 0;
        discreteActions[1] = 0;
        if (Input.GetKey(KeyCode.W))
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActions[1] = 1;
        }

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
       ActionSegment<int> discreteActions = actions.DiscreteActions;
       MoveHandle(discreteActions);
    }

    private void MoveHandle(ActionSegment<int>  direction)
    {
        _rb.velocity = new Vector3(0, 0, direction[0] == 1 ? 1 : direction[1] == 1 ? -1 : 0) * Speed;
    }

    public void Reset()
    {
        _rb ??= GetComponent<Rigidbody>();
        transform.localPosition = _startPos;
        _rb.velocity = Vector3.zero;
    }
}
