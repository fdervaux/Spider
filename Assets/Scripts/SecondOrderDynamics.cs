
using UnityEngine;
using Unity.Mathematics;

public class SecondOrderDynamics
{
    private Vector3 _xp;
    private Vector3 _y, _yd;

    private float _w, _z, _d, _k1, _k2, _k3;

    public SecondOrderDynamics(float f, float z, float r, Vector3 x0)
    {
        _w = 2 * Mathf.PI * f;
        _z = z;
        _d = _w * Mathf.Sqrt(Mathf.Abs(z * z - 1));

        _k1 = z / (Mathf.PI * f);
        _k2 = 1 / (_w * _w);
        _k3 = r * z / _w;

        _xp = Vector3.zero;
        _y = x0;
        _yd = Vector3.zero;
    }

    public Vector3 Update(float t, Vector3 x)
    {
        Vector3 xd = Vector3.zero;

        xd = (x - _xp) / t;
        _xp = x;

        return Update(t,x,xd);
    }

    public Vector3 Update(float t, Vector3 x, Vector3 xd)
    {
        

        float k1_stable, k2_stable;
        if (_w * t < _z)
        {
            k1_stable = _k1;
            k2_stable = Mathf.Max(_k2, t * t / 2 + t * _k1 / 2);
            k2_stable = Mathf.Max(k2_stable, t * _k1);
        }
        else
        {
            float t1 = Mathf.Exp(-_z * _w * t);
            float alpha = 2 * t1 * (_z <= 1 ? Mathf.Cos(t * _d) : math.cosh(t * _d));
            float beta = t1 * t1;
            float t2 = t / (1 + beta - alpha);

            k1_stable = (1 - beta) * t2;
            k2_stable = t * t2;
        }

        _y = _y + t * _yd;
        _yd = _yd + t * (x + _k3 * xd - _y - _k1 * _yd) / k2_stable;

        return _y;
    }

}