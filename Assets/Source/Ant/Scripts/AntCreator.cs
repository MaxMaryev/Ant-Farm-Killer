using System.Collections;
using UnityEngine;

public class AntCreator : MonoBehaviour
{
    [SerializeField] private int _count;
    [SerializeField] private Ant _antPrefab;
    [SerializeField] private Transform _nest;

    private void Start()
    {
        StartCoroutine(Create());
    }

    private IEnumerator Create()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        for (int i = 0; i < _count; i++)
        {
            Instantiate(_antPrefab);
            yield return wait;
        }
    }
}
