using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLegControl : MonoBehaviour
{

    [SerializeField] private Transform _refLegPos;
    [SerializeField] private Transform _targetLegPos;


    [SerializeField] private float _castDistance = 1;
    [SerializeField] private float _castRadius = 0.3f;
    [SerializeField] private LayerMask _castlayerMask;

    [SerializeField] private float _maxDistanceToFloorPoint = 0.2f;

    [SerializeField] private float _velocityAnticipationTime = 0.2f;

    private Vector3 _floorContactPoint = Vector3.zero;

    private bool _hasContactFloorPoint = false;
    private Rigidbody _rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        RaycastHit hit;
        _hasContactFloorPoint = false;

        Vector3 refPos = _refLegPos.position + _rigidbody.velocity * _velocityAnticipationTime;

        if (Physics.SphereCast(refPos, _castRadius, -transform.up, out hit, _castDistance, _castlayerMask))
        {
            _floorContactPoint = hit.point;
            _hasContactFloorPoint = true;
        }

        if(_hasContactFloorPoint && Vector3.Distance(_targetLegPos.position,_floorContactPoint) > _maxDistanceToFloorPoint)
        {
            _targetLegPos.position = _floorContactPoint;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        if(!_hasContactFloorPoint)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_floorContactPoint, 0.01f);
    }

}
