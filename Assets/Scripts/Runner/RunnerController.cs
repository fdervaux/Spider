using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class RunnerController : MonoBehaviour
{
    private int _currentPosition = 1;

    [SerializeField] float[] _shifts = new float[3];

    [SerializeField] private SecondOrder<Vector3> movementSecondOrder;
    [SerializeField] private CinemachineVirtualCamera _camera;

    private Vector3 _startPosition = Vector3.zero;

    private Rigidbody _rigidbody;

    private bool death = false;

    public bool Death { get => death; set => death = value; }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != LayerMask.GetMask("Floor"))
        {
            death = true;
            _camera.Follow = null;
        }
    }

    public void MoveLeft(CallbackContext context)
    {
        if (context.performed && _currentPosition != 0)
        {
            _currentPosition--;
        }

    }

    public void MoveRight(CallbackContext context)
    {
        if (context.performed && _currentPosition != 2)
        {
            _currentPosition++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!death)
            _rigidbody.MovePosition(SecondOrderDynamics.SencondOrderUpdate(_startPosition + Vector3.right * _shifts[_currentPosition], movementSecondOrder, Time.fixedDeltaTime));
        //Debug.Log(_currentPosition);
    }
}
