public class Vector
{
    float value;

    public float x { get { return value; } }

    public Vector(float value)
    {
        this.value = value;
    }

    public static implicit operator Vector(float value)
    {
        return new Vector(value);
    }

    public static Vector operator /(Vector first, float second)
    {
        return new Vector(first.value / second);
    }

    public static Vector operator *(float first, Vector second)
    {
        return new Vector(first * second.value);
    }

    public static Vector operator -(float first, Vector second)
    {
        return new Vector(first - second.value);
    }

    public static Vector operator -(Vector first, float second)
    {
        return new Vector(first.value - second);
    }

    public static Vector operator +(Vector first, float second)
    {
        return new Vector(first.value + second);
    }

    public static Vector operator +(Vector first, Vector second)
    {
        return new Vector(first.value + second.value);
    }

    public static Vector operator -(Vector first, Vector second)
    {
        return new Vector(first.value - second.value);
    }
}
