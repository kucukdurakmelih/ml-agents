using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PongBall : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; set; } = 5f;
    private Rigidbody _rb;

    private Vector3 _dir;

    private Action<Team> _goalTouched;
    // Start is called before the first frame update

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Init(Action<Team> goalTouched)
    {
        _goalTouched = goalTouched;
        Reset();
    }


    public void Reset()
    {
        _dir = new Vector3(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f)).normalized;
        transform.localPosition = new Vector3(-14, 0.5f, 0f);
    }


    private void FixedUpdate()
    {
        _rb.velocity = _dir * Speed;
    }


    private void OnCollisionEnter(Collision other)
    {
        var collisionNormal = other.contacts[0].normal;

        var direction = Vector3.Reflect(_dir, collisionNormal);
        _dir = direction;

        _rb.velocity = Vector3.zero;
        _rb.AddForce(_dir * Speed, ForceMode.Acceleration);


        if (other.gameObject.CompareTag("BlueGoal"))
        {
            _goalTouched?.Invoke(Team.Blue);
        }

        if (other.gameObject.CompareTag("PurpleGoal"))
        {
            _goalTouched?.Invoke(Team.Purple);
        }
    }
}
