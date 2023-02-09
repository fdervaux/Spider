using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLegControl : MonoBehaviour
{

    [SerializeField] private List<LegData> _legs;
    [SerializeField] private float _castDistance = 1;
    [SerializeField] private float _castRadius = 0.3f;
    [SerializeField] private LayerMask _castlayerMask;

    [SerializeField] private float _maxDistanceToFloorPoint = 0.2f;

    [SerializeField] private float _velocityAnticipationFactor = 1f;

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

        float velocityFactor = ExtensionMethods.Remap(_rigidbody.velocity.magnitude, 0, 4, 0, 1);
        float animationDuration = Mathf.Lerp(_minAnimationDuration,_maxAnimationDuration,velocityFactor);

        foreach (LegData leg in _legs)
        {
            if (leg.IsAnimated)
                legMoving = true;

            leg.FindTarget(
            animationDuration * _velocityAnticipationFactor,
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

        legToMove?.StartAnimation(animationDuration);
    }



    // Update is called once per frame
    void Update()
    {

        float velocityFactor = ExtensionMethods.Remap(_rigidbody.velocity.magnitude, 0, 4, 0, 1);
        float animationDuration = Mathf.Lerp(_minAnimationDuration,_maxAnimationDuration,velocityFactor);

        foreach (LegData leg in _legs)
        {
            leg.Animate(_animationCurve, animationDuration);
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
