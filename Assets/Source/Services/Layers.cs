using UnityEngine;

public static class Layers
{
    public static int Food { get => LayerMask.GetMask(nameof(Food)); }
    public static int TakenFood { get => LayerMask.NameToLayer(nameof(TakenFood)); }
    public static int FoodMarker { get => LayerMask.NameToLayer(nameof(FoodMarker)); } 
    public static int HomeMarker { get => LayerMask.NameToLayer(nameof(HomeMarker)); } 
}
