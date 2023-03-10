using UnityEngine;

[System.Serializable]
public class LegData
{
    [SerializeField] private Transform _refLegPos;
    [SerializeField] private Transform _targetLegPos;
    [SerializeField] private Transform _hintLegPos;
    [SerializeField] private Transform _baseLegPos;

    private float _animationTimeRemaining = 0;



    private Vector3 _startAnimationPosition;
    private Vector3 _targetAnimationPosition;

    private bool _hasContactFloorPoint = false;
    private Vector3 _floorContactPoint = Vector3.zero;

    public bool IsAnimated { get { return _animationTimeRemaining > 0; } }

    public float targetDistance { get { return Vector3.Distance(_targetLegPos.position, _targetAnimationPosition); } }

    public float heightShift { get { return _targetLegPos.position.y - _floorContactPoint.y; } }

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
    }


    public void UpdateHintPos()
    {
        Vector3 hintPos = _baseLegPos.position + (_targetLegPos.position - _baseLegPos.position) / 2;
        hintPos.y += 1f;
        _hintLegPos.position = hintPos;
    }

    public void Animate(AnimationCurve animationCurve, float animationDuration, AnimationCurve heightAnimationCurve, float heightAnimation)
    {
        if (_animationTimeRemaining > 0)
        {
            float factor = animationCurve.Evaluate(1 - _animationTimeRemaining / animationDuration);
            float height = heightAnimationCurve.Evaluate(factor) * heightAnimation;

            _targetLegPos.position = Vector3.Lerp(_startAnimationPosition, _targetAnimationPosition, factor) + Vector3.up * height;
            _animationTimeRemaining -= Time.deltaTime;

            if (_animationTimeRemaining <= 0)
            {
                _targetLegPos.position = _targetAnimationPosition;
            }
        }

        UpdateHintPos();
    }
}
