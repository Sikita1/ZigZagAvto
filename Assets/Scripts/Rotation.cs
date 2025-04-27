using UnityEngine;

public class Rotation : MonoBehaviour
{
    private float _speedRotation = 10f;

    private void Update()
    {
        Rotate();
    }

    public void Rotate() 
    {
        transform.Rotate(new Vector3(0, _speedRotation, 0) * Time.deltaTime);
    }
}
