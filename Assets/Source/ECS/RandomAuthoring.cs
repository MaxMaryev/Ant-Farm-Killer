using Unity.Entities;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class RandomAuthoring : MonoBehaviour
{
    public uint Seed;
}

public class RandomBaker : Baker<RandomAuthoring>
{
    public override void Bake(RandomAuthoring authoring)
    {
        AddComponent(new IndividualRandomData
        {
            Value = Unity.Mathematics.Random.CreateFromIndex(authoring.Seed)
        });
    }
}
