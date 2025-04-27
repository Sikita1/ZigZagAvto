using UnityEngine;

public class Diamond : Item
{
    [SerializeField] private ParticleSystem _diamondBlast;

    protected override void Blast()
    {
        GameManager.Instance.RaiseDiamond();
        Instantiate(_diamondBlast, transform.position, Quaternion.identity);
        base.Blast();
    }
}