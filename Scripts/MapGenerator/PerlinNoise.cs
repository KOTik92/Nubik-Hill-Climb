using UnityEngine;

public static class PerlinNoise
{
    private static int[] _permutation = new int[512];
    
    static PerlinNoise()
    {
        for (int i = 0; i < 256; i++)
        {
            _permutation[i] = i;
        }
        
        for (int i = 0; i < 256; i++)
        {
            var randomIndex = Random.Range(0, 256);
            (_permutation[i], _permutation[randomIndex]) = (_permutation[randomIndex], _permutation[i]);
        }

        for (int i = 0; i < 256; i++)
        {
            _permutation[i + 256] = _permutation[i];
        }
    }

    public static float GetPerlin2D(float x, float y)
    {
        // Find the unit grid cell containing the point
        int X = Mathf.FloorToInt(x) & 255;
        int Y = Mathf.FloorToInt(y) & 255;

        // Get relative xyz coordinates of point within that cell
        x -= Mathf.Floor(x);
        y -= Mathf.Floor(y);

        // Compute fade curves for each of x, y
        float u = Fade(x);
        float v = Fade(y);

        // Hash coordinates of the 4 square corners
        int A = _permutation[X] + Y;
        int AA = _permutation[A];
        int AB = _permutation[A + 1];
        int B = _permutation[X + 1] + Y;
        int BA = _permutation[B];
        int BB = _permutation[B + 1];

        // Add blended results from 4 corners of the square
        float res = Lerp(v, Lerp(u, Grad(_permutation[AA], x, y), Grad(_permutation[BA], x - 1, y)),
            Lerp(u, Grad(_permutation[AB], x, y - 1), Grad(_permutation[BB], x - 1, y - 1)));

        return (res + 1) / 2; // Convert to 0-1 range
    }
    
    public static float GetPerlin1D(float x)
    {
        // Find the unit grid cell containing the point
        int X = Mathf.FloorToInt(x) & 255;

        // Get relative xyz coordinates of point within that cell
        x -= Mathf.Floor(x);

        // Compute fade curves for each of x, y
        float u = Fade(x);

        // Hash coordinates of the 4 square corners
        int A = _permutation[X];
        int B = _permutation[X + 1];

        // Add blended results from 4 corners of the square
        float res = Lerp(u, Grad(_permutation[A], x, 0), Grad(_permutation[B], x - 1, 0));

        return (res + 1) / 2; // Convert to 0-1 range
    }

    private static float Fade(float t)
    {
        // Fade function as defined by Ken Perlin
        // This eases coordinate values so that they will "ease" towards integral values.
        // This ends up smoothing the final output.
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private static float Lerp(float t, float a, float b)
    {
        // Linear interpolation function, or "lerp"
        // Essentially: returns a value "t" percent between a and b.
        return a + t * (b - a);
    }

    private static float Grad(int hash, float x, float y)
    {
        // Convert lower 4 bits of hash code into 12 gradient directions
        int h = hash & 15;
        float u = h < 8 ? x : y;
        float v = h < 4 ? y : h == 12 || h == 14 ? x : 0;
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }
    
    
    
}