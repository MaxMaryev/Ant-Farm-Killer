using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.Ant
{
    internal class PheromoneSystem
    {
        private Ant _ant;
        private Sensor _leftSensor;
        private Sensor _centerSensor;
        private Sensor _rightSensor;
        private Vector3 _desiredDirection;

        internal PheromoneSystem(Ant ant, Sensor leftSensor, Sensor centerSensor, Sensor rightSensor)
        {
            _ant = ant;
            _leftSensor = leftSensor;
            _centerSensor = centerSensor;
            _rightSensor = rightSensor;
        }

        private void HandlePheromoneSteering()
        {
            UpdateSensor(_leftSensor);
            UpdateSensor(_centerSensor);
            UpdateSensor(_rightSensor);

            if (_centerSensor.Value > Mathf.Max(_leftSensor.Value, _rightSensor.Value))
                _desiredDirection = Vector3.forward;
            else if (_leftSensor.Value > _rightSensor.Value)
                _desiredDirection = Vector3.left;
            else if(_rightSensor.Value > _leftSensor.Value)
                _desiredDirection = Vector3.right;
        }

        private void UpdateSensor(Sensor sensor)
        {
            sensor.UpdatePosition(_ant.transform.position, _ant.transform.forward);
            sensor.Value = 0;

            PheromoneMap map = (_ant.HasFood) ? new FoodMarkers() : new HomeMarkers();
            IReadOnlyCollection<Pheromone> pheromones = map.GetAllInCircle(sensor.Position, sensor.Radius);

            foreach (var pheromone in pheromones)
            {
                float lifetime = Time.time - pheromone.CreationTime;
                float evaporateAmount = Mathf.Max(1, lifetime / pheromone.EvaporateTime);
                sensor.Value += 1 - evaporateAmount;
            }
        }
    }
}
