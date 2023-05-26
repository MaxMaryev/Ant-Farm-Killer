using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS_Ants
{
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
                Vector3 randomInsideUnitCircle = UnityEngine.Random.insideUnitCircle;
                Vector3 randomDirection = new Vector3(randomInsideUnitCircle.x, 0, randomInsideUnitCircle.y);
                Ant ant = Instantiate(_antPrefab, _nest.transform.position, Quaternion.Euler(randomDirection)/*, _nest*/);
                _ants.Add(ant);
                yield return wait;
            }
        }
    }
}
