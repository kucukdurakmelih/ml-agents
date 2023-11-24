using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    private Action<Team> _goal;
    private Vector3 _startingPosition = Vector3.zero;
    private Rigidbody _rb;
    // Start is called before the first frame update
    private void Awake()
    {

    }

    public void Init(Action<Team> goal)
    {
        _goal = goal;
        Reset();
    }

    public void Reset()
    {
        _rb ??= GetComponent<Rigidbody>();
        transform.localPosition = new Vector3(0,1,0);
        _rb.velocity = Vector3.zero;
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
        _rb.AddForce(randomDirection * 5f, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BlueGoal"))
        {
            _goal(Team.Red);
        }
        else if (other.gameObject.CompareTag("RedGoal"))
        {
            _goal(Team.Blue);
        }
    }




}
