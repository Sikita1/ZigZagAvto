using UnityEngine;

public class CarAnim : MonoBehaviour
{
    [SerializeField] private Vector3 _finalPosition;

    private Vector3 _initialPosition;

    private float _speedRotation = 10f;
    private float _changeRate = 0.2f;
    private float _initialRotation = -150f;

    private void Awake()
    {
        _initialPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _finalPosition, _changeRate);
        transform.Rotate(new Vector3(0, _speedRotation, 0) * Time.deltaTime);
    }

    private void OnDisable()
    {
        transform.position = _initialPosition;
        transform.rotation = Quaternion.Euler(0, _initialRotation, 0);
    }
}
