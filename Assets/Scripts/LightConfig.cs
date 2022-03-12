using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LightConfig
{
    #region Public Properties
    [field: SerializeField]
    [field: Tooltip("Range of the light")]
    public float Range { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Intensity of the light")]
    public float Intensity { get; private set; }
    #endregion

    #region Public Methods
    public void Apply(Light light)
    {
        light.range = Range;
        light.intensity = Intensity;
    }
    public static LightConfig Lerp(LightConfig start, LightConfig end, float t)
    {
        t = Mathf.Clamp01(t);
        return LerpUnclamped(start, end, t);
    }
    public static LightConfig LerpUnclamped(LightConfig start, LightConfig end, float t)
    {
        // Store the resulting configuration
        LightConfig result = new LightConfig();

        // Lerp color range and intensity
        result.Range = Mathf.LerpUnclamped(start.Range, end.Range, t);
        result.Intensity = Mathf.LerpUnclamped(start.Intensity, end.Intensity, t);

        // Return result
        return result;
    }
    #endregion
}
