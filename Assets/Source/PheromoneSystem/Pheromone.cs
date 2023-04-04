using UnityEngine;

internal class Pheromone : MonoBehaviour
{
    internal float CreationTime { get; private set; }
    internal float EvaporateTime { get; private set; }

    private void Awake()
    {
        CreationTime = Time.time;
        EvaporateTime = 15;
    }
}
