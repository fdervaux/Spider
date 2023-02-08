using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followTargetDynamics : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private SecondOrderData<Vector3> _data;

    // Start is called before the first frame update
    void Start()
    {
    }

    void FixedUpdate()
    {
        transform.position = SecondOrderDynamics.SencondOrderUpdate(target.transform.position,_data,Time.fixedDeltaTime);
    }

    private void OnValidate() {
        _data.UpdateData();
    }

    private void OnEnable() {
        _data.UpdateData();
    }
}
