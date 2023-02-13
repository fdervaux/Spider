
using UnityEngine;


public static class SecondOrderDynamics
{
    public static Vector2 SencondOrderUpdate(Vector2 targetPosition, SecondOrder<Vector2> secondOrder, float deltaTime)
    {
        if (!secondOrder.IsInit)
            secondOrder.Init(targetPosition);

        Vector2 xd = (targetPosition - secondOrder.lastPosition) / deltaTime;
        secondOrder.lastPosition = targetPosition;

        return SencondOrderUpdate(targetPosition, xd, secondOrder, deltaTime);
    }
    public static Vector2 SencondOrderUpdate(Vector2 targetPosition, Vector2 targetVelocity, SecondOrder<Vector2> secondOrder, float deltaTime)
    {
        if (!secondOrder.IsInit)
            secondOrder.Init(targetPosition);

        secondOrder.Data.setDeltaTime(deltaTime);

        if (secondOrder.IsInit)
        {
            secondOrder.targetPosition = targetPosition;
            secondOrder.lastPosition = targetPosition;
        }

        secondOrder.targetPosition = secondOrder.targetPosition + deltaTime * secondOrder.targetVelocity;
        secondOrder.targetVelocity = secondOrder.targetVelocity + deltaTime * (targetPosition + secondOrder.Data.K3 * targetVelocity - secondOrder.targetPosition - secondOrder.Data.K1 * secondOrder.targetVelocity) / secondOrder.Data.K2_stable;

        return secondOrder.targetPosition;
    }

    public static Vector3 SencondOrderUpdate(Vector3 targetPosition, SecondOrder<Vector3> secondOrder, float deltaTime)
    {
        if (!secondOrder.IsInit)
            secondOrder.Init(targetPosition);

        Vector3 xd = (targetPosition - secondOrder.lastPosition) / deltaTime;
        secondOrder.lastPosition = targetPosition;

        return SencondOrderUpdate(targetPosition, xd, secondOrder, deltaTime);
    }
    public static Vector3 SencondOrderUpdate(Vector3 targetPosition, Vector3 targetVelocity, SecondOrder<Vector3> secondOrder, float deltaTime)
    {
        if (!secondOrder.IsInit)
            secondOrder.Init(targetPosition);

        secondOrder.Data.setDeltaTime(deltaTime);

        secondOrder.targetPosition = secondOrder.targetPosition + deltaTime * secondOrder.targetVelocity;
        secondOrder.targetVelocity = secondOrder.targetVelocity + deltaTime * (targetPosition + secondOrder.Data.K3 * targetVelocity - secondOrder.targetPosition - secondOrder.Data.K1 * secondOrder.targetVelocity) / secondOrder.Data.K2_stable;

        return secondOrder.targetPosition;
    }

    public static float SencondOrderUpdate(float targetPosition, SecondOrder<float> secondOrder, float deltaTime)
    {
        if (!secondOrder.IsInit)
            secondOrder.Init(targetPosition);

        float xd = (targetPosition - secondOrder.lastPosition) / deltaTime;
        secondOrder.lastPosition = targetPosition;

        return SencondOrderUpdate(targetPosition, xd, secondOrder, deltaTime);
    }
    public static float SencondOrderUpdate(float targetPosition, float targetVelocity, SecondOrder<float> secondOrder, float deltaTime)
    {
        if (!secondOrder.IsInit)
            secondOrder.Init(targetPosition);
            
        secondOrder.Data.setDeltaTime(deltaTime);

        secondOrder.targetPosition = secondOrder.targetPosition + deltaTime * secondOrder.targetVelocity;
        secondOrder.targetVelocity = secondOrder.targetVelocity + deltaTime * (targetPosition + secondOrder.Data.K3 * targetVelocity - secondOrder.targetPosition - secondOrder.Data.K1 * secondOrder.targetVelocity) / secondOrder.Data.K2_stable;

        return secondOrder.targetPosition;
    }
}
