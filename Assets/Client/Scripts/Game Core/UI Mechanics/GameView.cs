using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class GameView: NetworkBehaviour
{
    [SerializeField] private Text _timerText;

    [SerializeField] private Text _scoreText;

    private MatchController _matchController;

    private void Start()
    {
        if (isServer == true) return;

        _scoreText.text = "0:0";

        _matchController = MatchController.Instance;

        _matchController.OnUpdateClientTimer += UpdateTimerText;
        _matchController.OnUpdateMatchScore += UpdateMatchScore;
    }

    private void OnDisable()
    {
        _scoreText.text = "0:0";
        _timerText.text = "00:00";
    }

    private void UpdateTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        _timerText.text = String.Format("{00}:{01}", minutes, seconds);
    }

    private void UpdateMatchScore(int scorePlayer1, int scorePlayer2)
    {
        _scoreText.text = $"{scorePlayer1}:{scorePlayer2}";
    }
}
