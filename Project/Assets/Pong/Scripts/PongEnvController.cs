using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class PongEnvController : MonoBehaviour
{
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 5000;
    public List<Handle> AgentsList = new List<Handle>();
    public PongBall ball;

    private SoccerSettings m_SoccerSettings;


    private SimpleMultiAgentGroup _leftPongGroup;
    private SimpleMultiAgentGroup _rightPongGroup;


    private int m_ResetTimer;

    void Start()
    {
        // Initialize TeamManager
        _leftPongGroup = new SimpleMultiAgentGroup();
        _rightPongGroup = new SimpleMultiAgentGroup();

        foreach (var item in AgentsList)
        {
            item.Reset();
            if (item.team == Team.Blue)
            {
                _leftPongGroup.RegisterAgent(item);
            }
            else
            {
                _rightPongGroup.RegisterAgent(item);
            }
        }

        ball.Init(GoalTouched);
        ResetScene();
    }

    void FixedUpdate()
    {

        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            _leftPongGroup.GroupEpisodeInterrupted();
            _rightPongGroup.GroupEpisodeInterrupted();
            ResetScene();
        }

    }


    private void GoalTouched(Team scoredTeam)
    {
        if (scoredTeam == Team.Blue)
        {
            _leftPongGroup.SetGroupReward(1);
            _rightPongGroup.SetGroupReward(-1);
        }
        else
        {
            _rightPongGroup.SetGroupReward(1);
            _leftPongGroup.SetGroupReward(-1);
        }

        _rightPongGroup.EndGroupEpisode();
        _leftPongGroup.EndGroupEpisode();
        ResetScene();
    }


    public void ResetScene()
    {
        m_ResetTimer = 0;
        //Reset Agents
        foreach (var item in AgentsList)
        {
            item.Reset();
        }

        ball.Reset();
    }
}
