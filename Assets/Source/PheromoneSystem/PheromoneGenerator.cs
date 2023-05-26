using System.Collections;
using UnityEngine;
using ECS_Ants;

public class PheromoneGenerator : MonoBehaviour
{
    [SerializeField] private PheromoneData _wanderPheromone;
    [SerializeField] private PheromoneData _foodPheromone;
    [SerializeField] private AntCreator _antCreator;

    private void Start()
    {
        StartCoroutine(Generating());
    }

    public IEnumerator Generating()
    {
        yield return new WaitUntil(() => _antCreator.Ants.Count > 0);

        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while (true)
        {
            for (int i = 0; i < _antCreator.Ants.Count; i++)
            {
                if (_antCreator.Ants[i].IsFoodDetecting)
                    Instantiate(_wanderPheromone, _antCreator.Ants[i].transform.position, Quaternion.identity);
                else
                    Instantiate(_foodPheromone, _antCreator.Ants[i].transform.position, Quaternion.identity);

                //yield return null;
            }

            yield return waitForSeconds;
        }
    }
}
