using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float _speed;

    private bool _isFaceLeft;
    private bool _isFirstTab;

    private float _rotationAngle = 90f;

    private void Update()
    {
        if (GameManager.Instance.IsGameStarted())
        {
            Move();
            CheckInput();
        }

        if (transform.position.y <= -2f)
            GameManager.Instance.GameOver();
    }

    private void Move()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
            ChangeDirection();
    }

    private void ChangeDirection()
    {
        if (_isFaceLeft)
        {
            _isFaceLeft = false;
            ChangeRotationAngle(_rotationAngle);
        }
        else
        {
            _isFaceLeft = true;
            ChangeRotationAngle(0);
        }
    }

    private void ChangeRotationAngle(float angle)
    {
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
