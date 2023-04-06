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
        _leftSensor = new Sensor(Vector3.one * 0.3f, Vector3.forward * 0.6f + Vector3.left * 0.5f, _ant.transform);
        _centerSensor = new Sensor(Vector3.one * 0.3f, Vector3.forward * 0.8f, _ant.transform);
        _rightSensor = new Sensor(Vector3.one * 0.3f, Vector3.forward * 0.6f + Vector3.right * 0.5f, _ant.transform);
    }

    public void UpdateSensors()
    {
        _leftSensor.Update();
        _centerSensor.Update();
        _rightSensor.Update();
    }

    public Vector3 GetDesiredDirection()
    {
        if (_centerSensor.Value > Mathf.Max(_leftSensor.Value, _rightSensor.Value))
        {
            return _ant.transform.forward;
        }
        else if (_leftSensor.Value > _rightSensor.Value)
        {
            return -_ant.transform.right;
        }
        else if (_rightSensor.Value > _leftSensor.Value)
        {
            return _ant.transform.right;
        }
        
        return Vector3.zero;
    }
}
