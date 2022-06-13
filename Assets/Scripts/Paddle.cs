using System;
using  UnityEngine;

public class Paddle : MonoBehaviour
{
    public static Paddle _instance;
    public static Paddle Instance => _instance;

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
    
    private Camera _mainCamera = default;
    private float _paddleInitialY = default;
    private float _defaultPaddleWidth = 200;
    private float _defaultLeftClamp = 125;
    private float _defaultRightClamp = 1800;
    private SpriteRenderer _spriteRenderer = default;
    

    private void Start()
    {
        _mainCamera = FindObjectOfType<Camera>();
        _paddleInitialY = this.transform.position.y;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float paddleShift = (_defaultPaddleWidth - (_defaultPaddleWidth / 2) * this._spriteRenderer.size.x) / 2;
        float leftClamp = _defaultLeftClamp - paddleShift;
        float rightClamp = _defaultRightClamp + paddleShift;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mousePositionX = _mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0 , 0)).x;
        this.transform.position = new Vector3(mousePositionX, _paddleInitialY, 0);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<Ball>() != null)
        {
            Rigidbody2D ballRB = col.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = col.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
            ballRB.velocity = Vector2.zero;
            float difference = paddleCenter.x - hitPoint.x;
            if (hitPoint.x < paddleCenter.x)
            {
                ballRB.AddForce(new Vector2(- (Mathf.Abs(difference * 200)), BallManager.Instance._initialSpeed));
            }
            else
            {
                ballRB.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallManager.Instance._initialSpeed));
            }
        }
    }
}
