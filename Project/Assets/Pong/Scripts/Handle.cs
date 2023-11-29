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



        public override void OnActionReceived(ActionBuffers actions)
        {
           ActionSegment<float> continuousActions = actions.ContinuousActions;
           MoveHandle(continuousActions);
        }

        private void MoveHandle(ActionSegment<float>  direction)
        {
            _rb.velocity = new Vector3(0, 0, direction[0]) * Speed;
    }

    public void Reset()
    {
        _rb ??= GetComponent<Rigidbody>();
        transform.localPosition = _startPos;
        _rb.velocity = Vector3.zero;
    }
}
