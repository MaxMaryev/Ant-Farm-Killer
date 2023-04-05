using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntCreator : MonoBehaviour
{
    [SerializeField] private int _count;
    [SerializeField] private Ant _antPrefab;
    [SerializeField] private Transform _nest;

    private List<Ant> _ants = new List<Ant>();

    public IReadOnlyList<Ant> Ants => _ants;

    private void Start()
    {
        StartCoroutine(Create());
    }

    private IEnumerator Create()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        for (int i = 0; i < _count; i++)
        {
            Ant ant = Instantiate(_antPrefab, _nest.transform.position, Quaternion.identity);
            _ants.Add(ant);
            yield return wait;
        }
    }
}
