using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LegData
{
    [SerializeField] private Transform _refLegPos;
    [SerializeField] private Transform _targetLegPos;

    private float _animationTimeRemaining = 0;

    private Vector3 _startAnimationPosition;
    private Vector3 _targetAnimationPosition;

    private bool _hasContactFloorPoint = false;
    private Vector3 _floorContactPoint = Vector3.zero;

    private float _currentAnimationDuration = 1;

    public bool IsAnimated { get { return _animationTimeRemaining > 0; } }

    public float targetDistance { get { return Vector3.Distance(_targetLegPos.position, _targetAnimationPosition); } }

    public void FindTarget(
        float velocityAnticipationTime,
        float castDistance,
        float castRadius,
        float maxDistanceToFloorPoint,
        Vector3 castDirection,
        LayerMask castLayerMask,
        Vector3 bodyVelocity)
    {
        RaycastHit hit;
        _hasContactFloorPoint = false;

        Vector3 refPos = _refLegPos.position + bodyVelocity * velocityAnticipationTime;

        if (Physics.SphereCast(refPos, castRadius, castDirection, out hit, castDistance, castLayerMask))
        {
            _floorContactPoint = hit.point;
            _hasContactFloorPoint = true;
        }

        if (_hasContactFloorPoint)
        {
            _targetAnimationPosition = _floorContactPoint;
        }

    }

    public void StartAnimation(float animationDuration)
    {
        _startAnimationPosition = _targetLegPos.position;
        _animationTimeRemaining = animationDuration;
        _currentAnimationDuration = animationDuration;
    }


    public void Animate(AnimationCurve animationCurve)
    {
        if (_animationTimeRemaining > 0)
        {
            float factor = animationCurve.Evaluate(1 - _animationTimeRemaining / _currentAnimationDuration);
            _targetLegPos.position = Vector3.Lerp(_startAnimationPosition, _targetAnimationPosition, factor);
            _animationTimeRemaining -= Time.deltaTime;

            if (_animationTimeRemaining <= 0)
            {
                _targetLegPos.position = _targetAnimationPosition;
            }
        }
    }
}

public class SpiderLegControl : MonoBehaviour
{

    [SerializeField] private List<LegData> _legs;
    [SerializeField] private float _castDistance = 1;
    [SerializeField] private float _castRadius = 0.3f;
    [SerializeField] private LayerMask _castlayerMask;

    [SerializeField] private float _maxDistanceToFloorPoint = 0.2f;

    [SerializeField] private float _velocityAnticipationTime = 0.2f;

    private Rigidbody _rigidbody;

    [SerializeField] private float _minAnimationDuration = 0.5f;
    [SerializeField] private float _maxAnimationDuration = 0.5f;
    [SerializeField] private AnimationCurve _animationCurve;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        bool legMoving = false;

        foreach (LegData leg in _legs)
        {
            if (leg.IsAnimated)
                legMoving = true;

            leg.FindTarget(
            _velocityAnticipationTime,
            _castDistance, _castRadius,
            _maxDistanceToFloorPoint,
            -transform.up,
            _castlayerMask,
            _rigidbody.velocity
            );
        }

        if (legMoving)
            return;

        LegData legToMove = null;
        float MaxDistance = 0;


        foreach (LegData leg in _legs)
        {
            float distance = leg.targetDistance;
            if (distance > _maxDistanceToFloorPoint && distance > MaxDistance)
            {
                legToMove = leg;
                MaxDistance = distance;
            }

        }

        float velocityFactor = ExtensionMethods.Remap(_rigidbody.velocity.magnitude, 0, 6, 0, 1);
        float animationDuration = Mathf.Lerp(_minAnimationDuration,_maxAnimationDuration,velocityFactor);
        legToMove?.StartAnimation(animationDuration);
    }



    // Update is called once per frame
    void Update()
    {
        foreach (LegData leg in _legs)
        {
            leg.Animate(_animationCurve);
        }
    }


    private void OnDrawGizmos()
    {
        /*if (!_hasContactFloorPoint)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_floorContactPoint, 0.01f);*/
    }

}
