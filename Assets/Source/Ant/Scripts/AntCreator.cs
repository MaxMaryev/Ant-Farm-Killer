using Assets.Source.Ant;
using System.Collections;
using UnityEngine;
using Zenject;

public class AntCreator : MonoBehaviour
{
    [SerializeField] private int _count;

    private Ant _ant;

    [Inject]
    private void Construct(Ant ant)
    {
        _ant = ant;
    }

    private void Start()
    {
        StartCoroutine(Create());
    }

    private IEnumerator Create()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        for (int i = 0; i < _count; i++)
        {
            Instantiate(_ant);
            yield return wait;
        }
    }
}
