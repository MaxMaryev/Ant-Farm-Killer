using System.Collections;
using UnityEngine;

public class Ant : MonoBehaviour, ISensorable
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _steerStrenght;
    [SerializeField] private float _wanderStrenght;
    [SerializeField] private Transform _jawsTransform;

    private Vector3 _position;
    private Vector3 _velocity;
    private Vector3 _desiredDirection;
    private SensorsHandler _pheromoneSensors;
    private Transform _cargoTransform;

    public bool IsFoodDetecting { get; private set; } = true;
    public bool IsHouseDetecting { get; private set; } = false;
    public Transform RootTransform => transform;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position + transform.forward * 0.6f - transform.right * 0.5f, Vector3.one * 0.3f);
    //    Gizmos.DrawCube(transform.position + transform.forward * 0.8f, Vector3.one * 0.3f);
    //    Gizmos.DrawCube(transform.position + transform.forward * 0.6f + transform.right * 0.5f, Vector3.one * 0.3f);

    //    Gizmos.DrawCube(transform.position, Vector3.one * 0.1f);
    //}

    private void Awake()
    {
        _pheromoneSensors = new SensorsHandler(this);
    }

    private void Update()
    {
        _pheromoneSensors.UpdateSensors();
        Move();
    }

    public void OnBorderDetected() => _velocity *= -1f;

    public void OnFoodDetected() => StartCoroutine(GoingToGrab());

    public void OnHouseDetected() => StartCoroutine(GoingToUngrab());

    private void Move()
    {
        Vector3 randomInsideUnitCircle = Random.insideUnitCircle;
        Vector3 randomDirection = new Vector3(randomInsideUnitCircle.x, 0, randomInsideUnitCircle.y);
        _desiredDirection = (_desiredDirection + randomDirection * _wanderStrenght + _pheromoneSensors.GetDesiredDirection()).normalized;
        Vector3 desiredVelocity = _desiredDirection * _maxSpeed;
        Vector3 desiredSteeringForce = (desiredVelocity - _velocity) * _steerStrenght;
        Vector3 acceleration = Vector3.ClampMagnitude(desiredSteeringForce, _steerStrenght) / 1; //корень в ClampMagnitude

        _velocity = Vector3.ClampMagnitude(_velocity + acceleration * Time.deltaTime, _maxSpeed);
        _position += _velocity * Time.deltaTime;

        float angle = Mathf.Atan2(_velocity.x, _velocity.z) * Mathf.Rad2Deg;
        transform.SetPositionAndRotation(_position, Quaternion.Euler(0, angle, 0));
    }

    private IEnumerator GoingToGrab()
    {
        while (IsFoodDetecting)
        {
            Collider[] food = Physics.OverlapBox(transform.position, Vector3.one * 0.01f, transform.rotation, 1 << Layers.Food);

            if (food.Length > 0)
            {
                _cargoTransform = food[0].transform;
                _cargoTransform.gameObject.layer = Layers.TakenFood;
                _cargoTransform.position = _jawsTransform.position;
                _cargoTransform.SetParent(transform);
                IsFoodDetecting = false;
                IsHouseDetecting = true;
                _velocity *= -1f;
            }

            yield return null;
        }
    }

    private IEnumerator GoingToUngrab()
    {
        while (IsHouseDetecting)
        {
            Collider[] house = Physics.OverlapBox(transform.position, Vector3.one * 0.01f, transform.rotation, 1 << Layers.House);

            if (house.Length > 0)
            {
                Destroy(_cargoTransform.gameObject);
                _cargoTransform = null;
                IsHouseDetecting = false;
                IsFoodDetecting = true;
                _velocity *= -1f;
            }

            yield return null;
        }
    }
}