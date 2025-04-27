using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
            Blast();
    }

    protected virtual void Blast()
    {
        gameObject.SetActive(false);
    }
}
