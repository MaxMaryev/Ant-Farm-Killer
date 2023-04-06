using System.Collections;
using UnityEngine;

public class PheromoneGenerator : MonoBehaviour
{
    [SerializeField] private Pheromone _wanderPheromone;
    [SerializeField] private Pheromone _foodPheromone;
    [SerializeField] private AntCreator _antCreator;

    private void Start()
    {
        StartCoroutine(Generating());
    }

    public IEnumerator Generating()
    {
        yield return new WaitUntil(() => _antCreator.Ants.Count > 0);

        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

        while (true)
        {
            for (int i = 0; i < _antCreator.Ants.Count; i++)
            {
                if (_antCreator.Ants[i].IsSearchingFood)
                    Instantiate(_wanderPheromone, _antCreator.Ants[i].transform.position, Quaternion.identity);
                else
                    Instantiate(_foodPheromone, _antCreator.Ants[i].transform.position, Quaternion.identity);

                yield return null;
            }

            yield return waitForSeconds;
        }
    }
}
