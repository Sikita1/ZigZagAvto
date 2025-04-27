using UnityEngine;

public class Star : Item
{
    [SerializeField] private ParticleSystem _starBlast;

    protected override void Blast()
    {
        GameManager.Instance.GetStar();
        Instantiate( _starBlast, transform.position, Quaternion.identity);
        base.Blast();
    }
}