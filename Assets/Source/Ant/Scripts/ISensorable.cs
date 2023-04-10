using UnityEngine;

public interface ISensorable
{
    public bool IsFoodDetecting { get; }
    public bool IsHouseDetecting { get; }
    public Transform RootTransform { get; }

    public void OnFoodDetected();
    public void OnHouseDetected();
    public void OnBorderDetected();
}
