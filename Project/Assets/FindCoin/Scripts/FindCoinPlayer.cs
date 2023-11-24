using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Random = UnityEngine.Random;

public class FindCoinPlayer : Agent
{
    private float _speed = 5f;
    private Rigidbody _rb;
    private MeshRenderer _meshRenderer;

    [field: SerializeField] private GameObject[] Coins { get; set; }


    public override void OnEpisodeBegin()
    {
        foreach (var coin in Coins)
        {
            coin.SetActive(false);
        }

        Coins[Random.Range(0, Coins.Length)].SetActive(true);
        Reset();
    }

    private void Reset()
    {
        _rb ??= GetComponent<Rigidbody>();
        _meshRenderer ??= GetComponent<MeshRenderer>();

        _rb.velocity = Vector3.zero;
        transform.localPosition = new Vector3(0, 1.5f, 0f);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.ContinuousActions;
        var direction = new Vector3(vectorAction[0], 0, vectorAction[1]);
        Move(direction);
    }


    private void Move(Vector3 direction)
    {
        _rb.velocity = direction * _speed;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            SetReward(1f);
            other.gameObject.SetActive(false);
            EndEpisode();
        }
    }

    void FixedUpdate()
    {
        if (StepCount >= MaxStep - 1)
        {
            SetReward(-1);
            EndEpisode();

        }
    }
}
