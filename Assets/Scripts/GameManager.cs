using System;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject _gameOver;
    public GameObject _vitoryScreen;
    public static GameManager _instance;
    public static GameManager Instance => _instance;
    public int _lives { get; set; }
    public int _aviableLifes = 3;
    public static event Action<int> OnLivesLost;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    public bool IsGameStarted { get; set; }

    private void Start()
    {
        this._lives = this._aviableLifes;
        Screen.SetResolution(1920,1080, true);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestroy += OnBrickDestroy;
    }

    private void OnBrickDestroy(Brick obj)
    {
        if (BrickManager.Instance._remainingBricks.Count <= 0)
        {
            BallManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BrickManager.Instance.LoadNextLevel();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBallDeath(Ball obj)
    {
        if (BallManager.Instance._Balls.Count <= 0)
        {
            this._lives--;
            if (this._lives < 1)
            {
                _gameOver.SetActive(true);
            }
            else
            {
                OnLivesLost?.Invoke(this._lives);
                BallManager.Instance.ResetBalls();
                IsGameStarted = false;
                BrickManager.Instance.LoadLevel(BrickManager.Instance._currentLevel);
            }
        }
    }

    private void OnDisable()
    {
        Ball.OnBallDeath -= OnBallDeath;
    }

    public void ShowVictory()
    {
        _vitoryScreen.SetActive(true);
    }
}
