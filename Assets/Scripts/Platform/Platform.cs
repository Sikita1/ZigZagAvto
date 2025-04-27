using UnityEngine;

public class Platform : MonoBehaviour
{
    private BlastParticle _particle;

    private float _delayedDeletion = 0.5f;
    private float _delayInvoke = 0.2f;

    private Star _star;
    private Diamond _diamond;

    private int _maxRandom = 51;

    private int _chanceStar = 4;
    private int _chanceDiamond = 7;

    private void Awake()
    {
        _particle = gameObject.GetComponentInChildren<BlastParticle>(includeInactive: true);
        _star = gameObject.GetComponentInChildren<Star>(includeInactive: true);
        _diamond = gameObject.GetComponentInChildren<Diamond>(includeInactive: true);
    }

    private void Start()
    {
        int chance = GetRandomChance();

        if(chance < _chanceStar)
            _star.gameObject.SetActive(true);

        if(chance == _chanceDiamond)
            _diamond.gameObject.SetActive(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
            Invoke("FallDown", _delayInvoke);
    }

    private void FallDown()
    {
        if (_particle != null)
            _particle.gameObject.SetActive(true);

        GetComponent<Rigidbody>().isKinematic = false;
        Destroy(gameObject, _delayedDeletion);
    }

    private int GetRandomChance() => 
        Random.Range(1, _maxRandom);
}
