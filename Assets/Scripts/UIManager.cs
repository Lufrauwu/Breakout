using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI _target;
    public TextMeshProUGUI _scoreText;
    public TextMeshProUGUI _livesText;
    public int Score { get; set; }

    private void Awake()
    {
        Brick.OnBrickDestroy += OnBrickDestroy;
        BrickManager.OnLevelLoad += OnLevelLoad;
        GameManager.OnLivesLost += OnLivesLost;
    }

    private void Start()
    {
        OnLivesLost(GameManager.Instance._aviableLifes);
    }

    private void OnLivesLost(int remainingLives)
    {
        _livesText.text = $@"Lives: {remainingLives}";
    }

    private void OnLevelLoad()
    {
        UpdateRemainBricksText();
        UpdateScoreText(0);
    }

    private void UpdateScoreText(int increment)
    {
        this.Score += increment;
        string scoreString = this.Score.ToString().PadLeft(5, '0');
        _scoreText.text = $@"Score:
{scoreString}";
    }

    private void OnBrickDestroy(Brick obj)
    {
        UpdateRemainBricksText();
        UpdateScoreText(10);
    }

    private void UpdateRemainBricksText()
    {
        _target.text = $@"Target:
{BrickManager.Instance._remainingBricks.Count} / {BrickManager.Instance._initialBricksCount}";
    }

    private void OnDisable()
    {
        Brick.OnBrickDestroy -= OnBrickDestroy;
        BrickManager.OnLevelLoad -= OnLevelLoad;
    }
}
