using System;
using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Ball"))
        {
            Ball ball = collider.GetComponent<Ball>();
            BallManager.Instance._Balls.Remove(ball);
            ball.Die();
        }
    }
}
