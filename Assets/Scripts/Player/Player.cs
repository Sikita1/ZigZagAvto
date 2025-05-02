using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _price;

    public int GetPrice() =>
        _price;
}
