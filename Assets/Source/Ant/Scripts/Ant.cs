using System.Collections;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _steerStrenght;
    [SerializeField] private float _wanderStrenght;
    [SerializeField] private float _viewRadius;
    [SerializeField] private float _viewAngle;
    [SerializeField] private Transform _headTransform;

    private Vector3 _position;
    private Vector3 _velocity;
    private Vector3 _desiredDirection;
    private Transform _targetFood;
    private PheromoneSensors _pheromoneSensors;

    public bool IsSearchingFood { get; private set; } = true;

    public Transform HeadTransfrom => _headTransform;

    private Vector3 dir;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + transform.forward * 0.6f - transform.right * 0.5f, Vector3.one * 0.3f);
        Gizmos.DrawCube(transform.position + transform.forward * 0.8f, Vector3.one * 0.3f);
        Gizmos.DrawCube(transform.position + transform.forward * 0.6f + transform.right * 0.5f, Vector3.one * 0.3f);
        Gizmos.DrawRay(transform.position, dir * 10);
    }

    private void Awake()
    {
        _pheromoneSensors = new PheromoneSensors(this);
    }

    private void Update()
    {
        _pheromoneSensors.UpdateSensors();
        Move();

        if (IsSearchingFood)
            HandleFood();
    }

    private void HandleFood()
    {
        if (_targetFood == null)
        {
            Collider[] allFood = Physics.OverlapSphere(_headTransform.position, _viewRadius, Layers.Food);

            if (allFood.Length > 0)
            {
                Transform food = allFood[Random.Range(0, allFood.Length)].transform;
                Vector3 foodDirection = (food.position - _headTransform.position).normalized;

                if (Vector3.Angle(Vector3.forward, foodDirection) < _viewAngle / 2)
                {
                    food.gameObject.layer = Layers.TakenFood;
                    _targetFood = food;
                }
            }
        }
        else
        {
            _desiredDirection = new Vector3((_targetFood.position - _headTransform.position).x, 0, (_targetFood.position - _headTransform.position).z).normalized;

            const float FoodPickUpRadius = 0.05f;

            if (Vector3.Distance(_targetFood.position, _headTransform.position) < FoodPickUpRadius)
            {
                _targetFood.position = _headTransform.position;
                _targetFood.SetParent(_headTransform);
                _targetFood = null;
                IsSearchingFood = false;

                _desiredDirection = new Vector3(_desiredDirection.x, _desiredDirection.y, -_desiredDirection.z);
            }

        }
    }

    private void Move()
    {
        Vector3 randomInsideUnitCircle = Random.insideUnitCircle;
        Vector3 randomDirection = new Vector3(randomInsideUnitCircle.x, 0, randomInsideUnitCircle.y);
        dir = _pheromoneSensors.GetDesiredDirection();
        _desiredDirection = (_desiredDirection + randomDirection * _wanderStrenght + _pheromoneSensors.GetDesiredDirection()).normalized;
        Vector3 desiredVelocity = _desiredDirection * _maxSpeed;
        Vector3 desiredSteeringForce = (desiredVelocity - _velocity) * _steerStrenght;
        Vector3 acceleration = Vector3.ClampMagnitude(desiredSteeringForce, _steerStrenght) / 1; //корень в ClampMagnitude

        _velocity = Vector3.ClampMagnitude(_velocity + acceleration * Time.deltaTime, _maxSpeed);
        _position += _velocity * Time.deltaTime;

        float angle = Mathf.Atan2(_velocity.x, _velocity.z) * Mathf.Rad2Deg;
        transform.SetPositionAndRotation(_position, Quaternion.Euler(0, angle, 0));
    }
}