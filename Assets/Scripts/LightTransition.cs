using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTransition : MonoBehaviour
{
    #region Private Properties
    private float TimeSinceEffectStart => Time.time - timeOfEffectStart;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Light to configure during the transition")]
    private new Light light;
    [SerializeField]
    [Tooltip("Color gradient used for the transition")]
    private Gradient gradient;
    [SerializeField]
    [Tooltip("Starting light configuration")]
    private LightConfig start;
    [SerializeField]
    [Tooltip("Ending light configuration")]
    private LightConfig end;
    [SerializeField]
    [Tooltip("Curve used to interpolate between the light configurations")]
    private AnimationCurve animationCurve;
    [SerializeField]
    [Tooltip("Duration of the transition")]
    private float duration = 1f;
    [SerializeField]
    [Tooltip("Play the effect when it wakes up")]
    private bool playOnAwake = true;
    [SerializeField]
    [Tooltip("If true, the transition destroys itself once it is finished playing")]
    private bool destroyOnFinish = false;
    [SerializeField]
    [Tooltip("Play the effect in reverse, going from end to start")]
    private bool reverse = false;

    [Space]

    [SerializeField]
    [Tooltip("Interpolator used to animate the transition")]
    [Range(0f, 1f)]
    private float interpolator = 0f;
    #endregion

    #region Private Fields
    private float timeOfEffectStart = float.MinValue;
    #endregion

    #region Public Methods
    public void Play()
    {
        reverse = false;
        timeOfEffectStart = Time.time;
    }
    public void PlayReversed()
    {
        reverse = true;
        timeOfEffectStart = Time.time;
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        if (playOnAwake)
        {
            if (reverse) PlayReversed();
            else Play();
        }
        // If we should not play on awake then make the start time the min number
        else timeOfEffectStart = float.MinValue;
    }
    private void Update()
    {
        // Check if we are still playing the effect
        if (TimeSinceEffectStart <= duration)
        {
            interpolator = TimeSinceEffectStart / duration;
            Animate();
        }
        // If the animation is finished and we should destroy ourselves,
        // then destroy ourselves
        else if (destroyOnFinish) Destroy(gameObject);
    }
    public void OnValidate()
    {
        if (light)
        {
            Animate();
        }
    }
    #endregion

    #region Private Methods
    private void Animate()
    {
        // Evaluate the animation curve used to lerp between light configs
        float t = animationCurve.Evaluate(interpolator);
        Color color;
        LightConfig config;

        // If animation is reversed then lerp from end to start
        if (reverse)
        {
            color = gradient.Evaluate(1 - interpolator);
            config = LightConfig.LerpUnclamped(end, start, t);
        }
        // If the animation is normal then lerp from start to end
        else
        {
            color = gradient.Evaluate(interpolator);
            config = LightConfig.LerpUnclamped(start, end, t);
        }

        // Set the light color to the correct gradient color
        light.color = color;

        // Apply the config to the light
        config.Apply(light);
    }
    #endregion

}
