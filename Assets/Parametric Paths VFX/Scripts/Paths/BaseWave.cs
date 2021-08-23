using System;
using NaughtyAttributes;
using UnityEngine;

public abstract class BaseWave : BasePathModule
{
    [Serializable]
    public enum WaveTypes
    {
        Sine,
        Square,
        Sawtooth,
        Triangle,
        SmoothSquare,
        Noise
    }

    public WaveTypes WaveType;
    public float amplitude = 1f;
    public float frequency = 1f;
    public float phase = 0;
    
    public bool UsesDelta => WaveType==WaveTypes.SmoothSquare || WaveType==WaveTypes.Noise;
    public bool UsesSeed => WaveType==WaveTypes.Noise;
    
    [ShowIf(nameof(UsesDelta))]
    public float delta = 0.01f;

    [ShowIf(nameof(UsesSeed))]
    public float seed;

    protected float CalcWave(float t)
    {
        float val;
        t = (t + phase) % 1f * frequency;
        switch (WaveType)
        {
            case WaveTypes.Sine:
                val = Mathf.Sin(t * Mathf.PI * 2f);
                val *= amplitude;
                break;
            case WaveTypes.Square:
                val = (t % 1f) < 0.5f ? 1 : -1;
                val *= amplitude;
                break;
            case WaveTypes.Sawtooth:
                val = (t % 1f) * 2f - 1;
                val *= amplitude;
                break;
            case WaveTypes.Triangle:
                t = t % 1f;
                val = (t < 0.5f ? t : 1 - t) - 0.25f;
                val *= amplitude * 4;
                break;
            case WaveTypes.SmoothSquare:
                val = (2f/Mathf.PI)*Mathf.Atan(Mathf.Sin(2 * Mathf.PI * t)/delta);
                val *= amplitude + (delta/2f);
                break;
            case WaveTypes.Noise:
                float theta = t * Mathf.PI * 2f;
                val = Mathf.PerlinNoise(
                    seed + delta * Mathf.Cos(theta) + 1,
                    seed + delta * Mathf.Sin(theta) + 1
                ) * 2f - 1f;
                val *= amplitude;
                break;
            default:
                val = 0;
                break;
        }

        return val;
    }
    
}