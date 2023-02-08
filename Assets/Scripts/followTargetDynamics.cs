using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTargetDynamics : MonoBehaviour
{

    [SerializeField] private float f, z, r;
    [SerializeField] private Transform target;

    SecondOrderDynamics secondOrderDynamics = null;

    // Start is called before the first frame update
    void Start()
    {
        secondOrderDynamics = new SecondOrderDynamics(f, z, r, transform.position);
    }

    void FixedUpdate()
    {
        transform.position = secondOrderDynamics.Update(Time.fixedDeltaTime, target.position);
    }
}
