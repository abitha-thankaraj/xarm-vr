using UnityEngine;

public static class Utils
{
    public static string Vector2ToString(Vector2 vector)
    {
        return $"{vector.x},{vector.y}";
    }

    public static string Vector3ToString(Vector3 vector)
    {
        return $"{vector.x},{vector.y},{vector.z}";
    }

    public static string QuaternionToString(Quaternion quaternion)
    {
        return $"{quaternion.x},{quaternion.y},{quaternion.z},{quaternion.w}";
    }

}
