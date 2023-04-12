using UnityEngine;

public static class Layers
{
    public static int Food { get => LayerMask.NameToLayer(nameof(Food)); }
    public static int TakenFood { get => LayerMask.NameToLayer(nameof(TakenFood)); }
    public static int FoodMark { get => LayerMask.NameToLayer(nameof(FoodMark)); } 
    public static int WanderMark { get => LayerMask.NameToLayer(nameof(WanderMark)); } 
    public static int Border { get => LayerMask.NameToLayer(nameof(Border)); } 
    public static int House { get => LayerMask.NameToLayer(nameof(House)); } 
}
