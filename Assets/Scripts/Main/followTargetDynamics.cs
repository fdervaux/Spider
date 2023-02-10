using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTargetDynamics : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private SecondOrder<Vector3> _secondOrder;
    [SerializeField] private SecondOrder<Vector3> _eulerAngleSecondOrder;

    [SerializeField] Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        transform.position = SecondOrderDynamics.SencondOrderUpdate(target.transform.position,_secondOrder,Time.fixedDeltaTime);

        Vector3 actualRotation = transform.rotation.eulerAngles;

        Vector3 targetAngle = new Vector3(_rigidbody.velocity.z * 15/4, 0, -_rigidbody.velocity.x * 15/4);
        actualRotation = SecondOrderDynamics.SencondOrderUpdate(targetAngle, _eulerAngleSecondOrder, Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(actualRotation);

    }

    private void OnEnable() {
        _secondOrder.Init();
        _eulerAngleSecondOrder.Init();
    }
}
