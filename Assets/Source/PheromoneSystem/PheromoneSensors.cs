using System;
using UnityEngine;

public class PheromoneSensors
{
    private Ant _ant;
    private Sensor _leftSensor;
    private Sensor _centerSensor;
    private Sensor _rightSensor;

    public PheromoneSensors(Ant ant)
    {
        _ant = ant;
        _leftSensor = new Sensor(Vector3.one, ant.transform.forward + Vector3.left);
        _centerSensor = new Sensor(Vector3.one, ant.transform.forward);
        _rightSensor = new Sensor(Vector3.one, ant.transform.forward + Vector3.left);
    }

    public void UpdateSensors()
    {
        _leftSensor.Update(_ant.HeadTransfrom.position, _ant.HeadTransfrom.forward, _ant.IsSearchingFood);
        _centerSensor.Update(_ant.HeadTransfrom.position, _ant.HeadTransfrom.forward, _ant.IsSearchingFood);
        _rightSensor.Update(_ant.HeadTransfrom.position, _ant.HeadTransfrom.forward, _ant.IsSearchingFood);
    }

    public Vector3 GetDesiredDirection()
    {
        if (_centerSensor.Value > Mathf.Max(_leftSensor.Value, _rightSensor.Value))
            return Vector3.forward;
        else if (_leftSensor.Value > _rightSensor.Value)
            return Vector3.left;
        else if (_rightSensor.Value > _leftSensor.Value)
            return Vector3.right;

        return Vector3.zero;
    }
}
