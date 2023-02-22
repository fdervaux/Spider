using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class RunnerController : MonoBehaviour
{
    [SerializeField] float[] _shifts = new float[3];
    [SerializeField] private SecondOrder<Vector3> _movementSecondOrder;
    [SerializeField] private CinemachineVirtualCamera _camera;
    private int _currentPosition = 1;
    private Vector3 _startPosition = Vector3.zero;
    private Rigidbody _rigidbody;
    private bool death = false;
    public bool Death { get => death; set => death = value; }

    private void OnCollisionEnter(Collision other)
    {
        OnDie(other);
    }

    private void OnDie(Collision other)
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
            if (!death)
            {
                _currentPosition--;
            }
        }
    }

    public void MoveRight(CallbackContext context)
    {
        if (context.performed && _currentPosition != 2)
        {
            if (!death)
            {
                _currentPosition++;
            }
        }
    }
    
    void Start()
    {
        InitialiseController();
    }

    private void InitialiseController()
    {
        _startPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (!death)
        {
            _rigidbody.velocity = (SecondOrderDynamics.SencondOrderUpdate(_startPosition + Vector3.right * _shifts[_currentPosition], _movementSecondOrder, Time.fixedDeltaTime) - _rigidbody.position ) / Time.fixedDeltaTime;
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
            //_rigidbody.MovePosition(SecondOrderDynamics.SencondOrderUpdate(_startPosition + Vector3.right * _shifts[_currentPosition], _movementSecondOrder, Time.fixedDeltaTime));
    }
}
