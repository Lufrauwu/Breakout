using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private Ball _ballPrefab = default;
    public float _initialSpeed = 250;
    private Ball _initialBall = default;
    private Rigidbody2D _Initialrigidbody2D = default;
    public static BallManager _instance;
    public static BallManager Instance => _instance;

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
    public List<Ball> _Balls { get; set; }

    private void Start()
    {
        InitializeBall();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted)
        {
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + .4f, 0);
            _initialBall.transform.position = ballPosition;
            if (Input.GetMouseButtonDown(0))
            {
                _Initialrigidbody2D.isKinematic = false;
                _Initialrigidbody2D.AddForce(new Vector2(0, _initialSpeed));
                GameManager.Instance.IsGameStarted = true;
            }
        }
    }

    private void InitializeBall()
    {
        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + .45f, 0);
        _initialBall = Instantiate(_ballPrefab, startingPosition, Quaternion.identity);
        _Initialrigidbody2D = _initialBall.GetComponent<Rigidbody2D>();
        this._Balls = new List<Ball>
        {
            _initialBall
        };
    }

    public void ResetBalls()
    {
        foreach (var ball in this._Balls.ToList())
        {
            Destroy(ball.gameObject);
        }
        InitializeBall();
    }
}
