using UnityEngine;

public static class Layers
{
    public static int Food { get => LayerMask.GetMask(nameof(Food)); }
    public static int TakenFood { get => LayerMask.NameToLayer(nameof(TakenFood)); }
    public static int Pheromone { get => LayerMask.NameToLayer(nameof(Pheromone)); } 
    public static int Border { get => LayerMask.NameToLayer(nameof(Border)); } 
}
