using System.Collections;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private Platform _platform;
    [SerializeField] private Transform _lastPlatform;

    private Vector3 _lastPosition;
    private Vector3 _newPosition;

    private bool _isWork;

    private WaitForSeconds _wait;
    private float _delay = 0.2f;
    private float _stepPlatform = 2f;

    private Coroutine _coroutine;


    private void Awake()
    {
        _wait = new WaitForSeconds(_delay);
    }

    private void Start()
    {
        _lastPosition = _lastPlatform.position;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Spawn());
    }

    private void GeneratePosition()
    {
        _newPosition = _lastPosition;

        int random = Random.Range(0, 2);

        if (random > 0)
            _newPosition.z += _stepPlatform;
        else
            _newPosition.x += _stepPlatform;
    }

    private IEnumerator Spawn()
    {
        while(_isWork == false)
        {
            GeneratePosition();
            Instantiate(_platform,_newPosition, Quaternion.identity);
            _lastPosition = _newPosition;
            yield return _wait;
        }
    }
}
