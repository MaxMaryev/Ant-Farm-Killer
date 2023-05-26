using Unity.Mathematics;

public static class MathUtils
{
    public static float3 ClampFloat3(in float3 vector, in float maxLength)
    {
        float sqrMagnitude = math.lengthsq(vector);
        float sqrLength = maxLength * maxLength;

        if (sqrMagnitude > sqrLength)
        {
            float3 normalized = vector / sqrMagnitude;
            return normalized * sqrLength;
        }

        return vector;
    }
}
