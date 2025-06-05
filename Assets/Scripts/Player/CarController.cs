using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private ParticleSystem[] _particleSystems; 

    private bool _isFaceLeft;

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
        {
            ChangeDirection();
            StartSmoke();
        }
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

    private void StartSmoke()
    {
        for (int i = 0; i < _particleSystems.Length; i++)
            _particleSystems[i].Play();
    }
}
