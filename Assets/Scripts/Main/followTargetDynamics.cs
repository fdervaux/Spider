using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTargetDynamics : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private SecondOrder<Vector3> _secondOrder;
    [SerializeField] private SecondOrder<Vector3> _eulerAngleSecondOrder;
    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] private SpiderLegControl legController;

    [SerializeField] private float _heightFactor = 0.5f;
    [SerializeField] private SecondOrder<float> _heightDynamics;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = SecondOrderDynamics.SencondOrderUpdate(target.transform.position, _secondOrder, Time.deltaTime);

        Vector3 actualRotation = transform.rotation.eulerAngles;

        Vector3 targetAngle = new Vector3(_rigidbody.velocity.z * 20 / 4, 0, -_rigidbody.velocity.x * 20 / 4);
        actualRotation = SecondOrderDynamics.SencondOrderUpdate(targetAngle, _eulerAngleSecondOrder, Time.deltaTime);
        transform.rotation = Quaternion.Euler(actualRotation);

        float height =  SecondOrderDynamics.SencondOrderUpdate(legController.getHeihtShift() * _heightFactor, _heightDynamics, Time.deltaTime);
        transform.position = transform.position + Vector3.up * height;

    }

    private void OnEnable()
    {
        _secondOrder.Init(transform.position);
        _eulerAngleSecondOrder.Init(Vector3.zero);
        _heightDynamics.Init(0);
    }
}
