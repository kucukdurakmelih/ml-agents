using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class PlayerController : Agent
{
    [field: Range(0f, 20f)]
    [field: SerializeField]
    private float Speed { get; set; } = 5f;

    [field: SerializeField] public Team Team {  get; private set; }
    private Rigidbody _rb;
    private MeshRenderer _meshRenderer;
    private Vector3 _startingPos = Vector3.zero;
    private Action _episodeEndedCallback;
    private Action _episodeStartedCallback;
    private Action<Team> _touchedBallCallback;

    private const int GoalPoint = 1;

    private float _reward = 0f;

    private float Reward
    {
        get => _reward
        ;
        set => _reward = Mathf.Clamp(value, -1f, 1f)
        ;
    }
    // Start is called before the first frame update

    private void Start()
    {
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        Reset();
        _episodeStartedCallback?.Invoke();
        Reward = 0f;
    }


    public void Init(Action episodeStartedCallback, Action episodeEndedCallback, Action<Team> touchedBallCallback)
    {
        _episodeEndedCallback = episodeEndedCallback;
        _episodeStartedCallback = episodeStartedCallback;
        _touchedBallCallback = touchedBallCallback;
        Reset();
    }



    private void Reset()
    {
        _rb ??= GetComponent<Rigidbody>();
        _meshRenderer ??= GetComponent<MeshRenderer>();
        _startingPos = _startingPos == Vector3.zero ? transform.position : _startingPos;

        _rb.velocity = Vector3.zero;
        transform.position = _startingPos;
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.ContinuousActions;
        var direction = new Vector3(vectorAction[0], 0, vectorAction[1]);
        Move(direction);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (StepCount >= MaxStep - 1)
            CalculateReward();
    }

    private Vector3 CalculateDirection()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        return new Vector3(x, 0, z);
    }

    private void Move(Vector3 direction)
    {
        _rb.velocity = direction * Speed;
        var currentStep = (int)StepCount;
    }

    public void Goal(Team team)
    {
        if (team == Team)
        {
            UpdateReward(1);
        }
        else
        {
            UpdateReward(-1);
        }

        _episodeEndedCallback?.Invoke();
        EndEpisode();
    }

    public void CalculateReward()
    {
        IncreaseReward(-.5f);

        _episodeEndedCallback?.Invoke();
        EndEpisode();
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            TouchedBall();
            _touchedBallCallback?.Invoke(Team);
        }
    }

    private void TouchedBall()
    {
        if(Reward > .5f) return;
        IncreaseReward(.05f);
    }

    public void OtherPlayerTouchedBall()
    {
        IncreaseReward(-.05f);
    }

    private void UpdateReward(float value)
    {
        Reward = value;
        SetReward(value);
    }

    private void IncreaseReward(float value)
    {
        AddReward(value);
        Reward += value;
    }
}
