using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicSoccerGameController : MonoBehaviour
{
    [field: SerializeField] private List<PlayerController> Players { get; set; }
    [field: SerializeField] private BallController Ball { get; set; }
    [field: SerializeField] private Material NoGoalMaterial { get; set; }
    [field: SerializeField] private Material BlueGoalMaterial { get; set; }
    [field: SerializeField] private Material RedGoalMaterial { get; set; }

    [field: SerializeField ] private MeshRenderer MeshRenderer { get; set; }
    private EpisodeState _episodeState = EpisodeState.Ended;
    private Goaler _goaler = Goaler.None;

    private void Awake()
    {
        InitGame();
    }

    void Start()
    {
    }

    private void InitGame()
    {
        foreach (var playerController in Players)
        {
            playerController.Init(EpisodeStartedCallBack,EpisodeEndedCallBack,PlayerTouchedBall);
        }

        Ball.Init(Goal);
    }

    private void Goal(Team team)
    {
        foreach (var playerController in Players)
        {
            playerController.Goal(team);
        }

        MeshRenderer.material = team == Team.Blue ? BlueGoalMaterial : RedGoalMaterial;
        _goaler = team == Team.Blue ? Goaler.Blue : Goaler.Red;
    }

    private void EpisodeEndedCallBack()
    {
        if (_episodeState == EpisodeState.Ended) return;
        _episodeState = EpisodeState.Ended;

        if (_goaler == Goaler.None)
            MeshRenderer.material = NoGoalMaterial;
    }

    private void EpisodeStartedCallBack()
    {
        if (_episodeState == EpisodeState.Started) return;
        _episodeState = EpisodeState.Started;
        _goaler = Goaler.None;
        ResetBall();
    }

    private void ResetBall()
    {
        Ball.Reset();
    }

    private void PlayerTouchedBall(Team team)
    {
        PlayerController playerController = Players.FirstOrDefault(x => x.Team != team);
        playerController?.OtherPlayerTouchedBall();
    }
    // Update is called once per frame
}

public enum EpisodeState
{
    Started,
    Ended
}

public enum Goaler
{
    None,
    Blue,
    Red,
}
