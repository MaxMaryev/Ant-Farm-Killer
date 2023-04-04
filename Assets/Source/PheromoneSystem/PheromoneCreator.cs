using UnityEngine;

public class PheromoneCreator : MonoBehaviour
{
    [SerializeField] private Pheromone _pheromoneTemplate;
    [SerializeField] private AntCreator _antCreator;

    public void Create(Vector3 position)
    {
        Instantiate(_pheromoneTemplate, position, Quaternion.identity);
    }
}
