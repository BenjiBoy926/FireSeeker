using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightTransition : MonoBehaviour
{
    #region Private Properties
    private float TimeSinceEffectStart => Time.time - timeOfEffectStart;
    #endregion

    #region Private Editor Fields
    [field: SerializeField]
    [field: Tooltip("Light to configure during the transition")]
    public Light Light { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Color gradient used for the transition")]
    public Gradient Gradient { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Starting light configuration")]
    public LightConfig StartConfig { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Ending light configuration")]
    public LightConfig EndConfig { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Curve used to interpolate between the light configurations")]
    public AnimationCurve AnimationCurve { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Duration of the transition")]
    public float Duration { get; private set; } = 1f;
    [field: SerializeField]
    [field: Tooltip("Play the effect when it wakes up")]
    public bool PlayOnAwake { get; private set; } = true;
    [field: SerializeField]
    [field: Tooltip("Play the effect in reverse, going from end to start")]
    public bool Reverse { get; private set; } = false;

    [field: Space]

    [field: SerializeField]
    [field: Tooltip("Interpolator used to animate the transition")]
    [field: Range(0f, 1f)]
    public float Interpolator { get; private set; } = 0f;

    [field: Space]

    [field: SerializeField]
    [field: Tooltip("Event invoked when the transition finishes")]
    public UnityEvent TransitionCompleteEvent { get; private set; }
    #endregion

    #region Private Fields
    private float timeOfEffectStart = float.MinValue;
    private bool transitionCompleteEventInvoked = false;
    #endregion

    #region Public Methods
    public void Play()
    {
        transitionCompleteEventInvoked = false;
        Reverse = false;
        timeOfEffectStart = Time.time;
    }
    public void PlayReversed()
    {
        transitionCompleteEventInvoked = false;
        Reverse = true;
        timeOfEffectStart = Time.time;
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        if (PlayOnAwake)
        {
            if (Reverse) PlayReversed();
            else Play();
        }
        // If we should not play on awake then make the start time the min number
        else timeOfEffectStart = float.MinValue;
    }
    private void Update()
    {
        // Check if we are still playing the effect
        if (TimeSinceEffectStart <= Duration)
        {
            Interpolator = TimeSinceEffectStart / Duration;
            Animate();
        }
        // If the transition complete event has not been invoked yet,
        // then invoke the event
        else if (!transitionCompleteEventInvoked)
        {
            transitionCompleteEventInvoked = true;
            TransitionCompleteEvent.Invoke();
        }
    }
    public void OnValidate()
    {
        if (Light)
        {
            Animate();
        }
    }
    #endregion

    #region Private Methods
    private void Animate()
    {
        // Evaluate the animation curve used to lerp between light configs
        float t = AnimationCurve.Evaluate(Interpolator);

        // Color and config to set for the light
        Color color;
        LightConfig config;

        // If animation is reversed then lerp from end to start
        if (Reverse)
        {
            color = Gradient.Evaluate(1 - Interpolator);
            config = LightConfig.LerpUnclamped(EndConfig, StartConfig, t);
        }
        // If the animation is normal then lerp from start to end
        else
        {
            color = Gradient.Evaluate(Interpolator);
            config = LightConfig.LerpUnclamped(StartConfig, EndConfig, t);
        }

        // Set the light color to the correct gradient color
        Light.color = color;

        // Apply the config to the light
        config.Apply(Light);
    }
    #endregion

}
