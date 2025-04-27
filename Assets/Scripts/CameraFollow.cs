using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _followSpeed;

    private Vector3 _distance;

    private void Start()
    {
        _target = GameManager.Instance.GetPlayer().transform;
        _distance = transform.position - _target.position;
    }

    private void Update()
    {
        if (_target != null)
            if (_target.position.y >= 0)
                Follow();
    }

    private void Follow()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = _target.position - _distance;

        transform.position = Vector3.Lerp(currentPosition, targetPosition, _followSpeed * Time.deltaTime);
    }
}
