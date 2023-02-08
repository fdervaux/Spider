
using UnityEngine;
using Unity.Mathematics;

[System.Serializable]
public class SecondOrderData<T>
{
    [SerializeField] private float frequency;

    [SerializeField] private float damping;

    [SerializeField] private float impulse;

    private T _lastPosition;
    private T _targetPosition, _targetVelocity;

    private float _w, _z, _d, _k1, _k2, _k3;
    private float k1_stable, k2_stable;

    public SecondOrderData()
    {
        UpdateData();
    }

    public SecondOrderData(float frequency, float damping, float impulse)
    {
        this.frequency = frequency;
        this.damping = damping;
        this.impulse = impulse;

        UpdateData();
    }

    public T targetPosition { get => _targetPosition; set => _targetPosition = value; }
    public T targetVelocity { get => _targetVelocity; set => _targetVelocity = value; }
    public T lastPosition { get => _lastPosition; set => _lastPosition = value; }

    public float K1 { get => _k1; set => _k1 = value; }
    public float K2_stable { get => k2_stable; set => k2_stable = value; }
    public float K3 { get => _k3; set => _k3 = value; }

    public void UpdateData()
    {
        _w = 2 * Mathf.PI * frequency;
        _z = damping;
        _d = _w * Mathf.Sqrt(Mathf.Abs(damping * damping - 1));

        _k1 = damping / (Mathf.PI * frequency);
        _k2 = 1 / (_w * _w);
        _k3 = impulse * damping / _w;
    }

    public void setDeltaTime(float deltaTime)
    {
        if (_w * deltaTime < _z)
        {
            k1_stable = _k1;
            k2_stable = Mathf.Max(_k2, deltaTime * deltaTime / 2 + deltaTime * _k1 / 2);
            k2_stable = Mathf.Max(k2_stable, deltaTime * _k1);
        }
        else
        {
            float t1 = Mathf.Exp(-_z * _w * deltaTime);
            float alpha = 2 * t1 * (_z <= 1 ? Mathf.Cos(deltaTime * _d) : math.cosh(deltaTime * _d));
            float beta = t1 * t1;
            float t2 = deltaTime / (1 + beta - alpha);

            k1_stable = (1 - beta) * t2;
            k2_stable = deltaTime * t2;
        }
    }
}


public static class SecondOrderDynamics
{
    public static Vector3 SencondOrderUpdate(Vector3 targetPosition, SecondOrderData<Vector3> data, float deltaTime)
    {
        Vector3 xd = (targetPosition - data.lastPosition) / deltaTime;
        data.lastPosition = targetPosition;

        return SencondOrderUpdate(targetPosition, xd, data, deltaTime);
    }
    public static Vector3 SencondOrderUpdate(Vector3 targetPosition, Vector3 targetVelocity, SecondOrderData<Vector3> data, float deltaTime)
    {
        data.setDeltaTime(deltaTime);

        data.targetPosition = data.targetPosition + deltaTime * data.targetVelocity;
        data.targetVelocity = data.targetVelocity + deltaTime * (targetPosition + data.K3 * targetVelocity - data.targetPosition - data.K1 * data.targetVelocity) / data.K2_stable;

        return data.targetPosition;
    }
}
