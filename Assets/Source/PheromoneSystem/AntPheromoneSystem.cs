using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class AntPheromoneSystem
{
    private Ant _ant;
    private Sensor _leftSensor;
    private Sensor _centerSensor;
    private Sensor _rightSensor;

    internal event Action PheromoneLeft;

    internal AntPheromoneSystem(Ant ant)
    {
        _ant = ant;
        _leftSensor = new Sensor(Vector3.one, ant.transform.forward + Vector3.left);
        _centerSensor = new Sensor(Vector3.one, ant.transform.forward);
        _rightSensor = new Sensor(Vector3.one, ant.transform.forward + Vector3.left);
    }

    internal void UpdateSensors()
    {
        UpdateSensor(_leftSensor);
        UpdateSensor(_centerSensor);
        UpdateSensor(_rightSensor);
    }

    internal Vector3 GetDesiredDirection()
    {
        if (_centerSensor.Value > Mathf.Max(_leftSensor.Value, _rightSensor.Value))
            return Vector3.forward;
        else if (_leftSensor.Value > _rightSensor.Value)
            return Vector3.left;
        else if (_rightSensor.Value > _leftSensor.Value)
            return Vector3.right;

        return Vector3.zero;
    }

    private void UpdateSensor(Sensor sensor)
    {
        sensor.UpdatePosition(_ant.HeadTransfrom.position, _ant.HeadTransfrom.forward);
        sensor.Value = 0;

        PheromoneMap map = (_ant.IsSearchingFood) ? new FoodMarkers() : new HomeMarkers();
        IReadOnlyCollection<Pheromone> pheromones = map.GetDetectedPheromonesCount(sensor.Position, _ant.transform.rotation, sensor.HalfExtents);

        foreach (var pheromone in pheromones)
        {
            float lifetime = Time.time - pheromone.CreationTime;
            float evaporateAmount = Mathf.Max(1, lifetime / pheromone.EvaporateTime);
            sensor.Value += 1 - evaporateAmount;
        }
    }

    internal IEnumerator LeavePheromonesTrail()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        while (_ant.gameObject.activeSelf)
        {
            PheromoneLeft?.Invoke();
            yield return waitForSeconds;
        }
    }
}
