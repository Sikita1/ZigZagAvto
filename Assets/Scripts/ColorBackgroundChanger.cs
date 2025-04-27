using UnityEngine;

public class ColorBackgroundChanger : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] private float _lerpTime;
    [SerializeField] private Color[] _myColor;
    private int _colorIndex;
    private float _change = 0f;
    private int _len;

    private void Start()
    {
        _len = _myColor.Length;
    }

    private void Update()
    {
        Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, _myColor[_colorIndex], _lerpTime * Time.deltaTime);
        _change = Mathf.Lerp(_change, 1f, _lerpTime * Time.deltaTime);

        if (_change > 0.9f)
        {
            _change = 0f;
            _colorIndex++;

            _colorIndex = (_colorIndex >= _len) ? 0 : _colorIndex;
        }
    }
}
