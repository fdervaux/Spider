using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTargetDynamics : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private SecondOrder<Vector3> _secondOrder;
    [SerializeField] private SecondOrder<Vector3> _eulerAngleSecondOrder;
    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] private float _heightFrequency = 1;
    [SerializeField] private float _heightAmplitude = 0.5f;
    [SerializeField] private SecondOrder<float> _heightDynamics;

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

        /*float animationFactor = ExtensionMethods.Remap(_rigidbody.velocity.magnitude / 4, 0, 1, 0.5f, 1f);
        float heightTarget = Mathf.Sin(Time.time * _heightFrequency * animationFactor) * Mathf.Pow(_rigidbody.velocity.magnitude / 4, 1/3f);
        float height = SecondOrderDynamics.SencondOrderUpdate(heightTarget * _heightAmplitude, _heightDynamics, Time.deltaTime);
        transform.position = transform.position + Vector3.up * height;*/

    }
}
