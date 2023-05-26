using UnityEngine;

public class PheromoneData : MonoBehaviour
{
    [SerializeField] private int _lifeTime;

    private void Start()
    {
        InvokeRepeating(nameof(ReduceLifetime), _lifeTime, 1);
    }

    private void ReduceLifetime()
    {
        _lifeTime -= 1;

        if (_lifeTime <= 0)
            Destroy(gameObject);
    }
}
