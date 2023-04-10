using UnityEngine;

public class SensorsHandler
{
    private ISensorable _sensorable;
    private Sensor _leftSensor;
    private Sensor _centerSensor;
    private Sensor _rightSensor;

    public SensorsHandler(ISensorable sensorable)
    {
        _sensorable = sensorable;
        _leftSensor = new Sensor(Vector3.one * 0.3f, Vector3.forward * 0.6f + Vector3.left * 0.5f, _sensorable);
        _centerSensor = new Sensor(Vector3.one * 0.3f, Vector3.forward * 0.8f, _sensorable);
        _rightSensor = new Sensor(Vector3.one * 0.3f, Vector3.forward * 0.6f + Vector3.right * 0.5f, _sensorable);
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
            return _sensorable.RootTransform.forward;
        }
        else if (_leftSensor.Value > _rightSensor.Value)
        {
            return -_sensorable.RootTransform.right;
        }
        else if (_rightSensor.Value > _leftSensor.Value)
        {
            return _sensorable.RootTransform.right;
        }

        return Vector3.zero;
    }
}
