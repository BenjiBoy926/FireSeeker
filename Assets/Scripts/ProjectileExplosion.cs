using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    #region Private Properties
    private float TimeSinceEffectStart => Time.time - timeOfEffectStart;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the light shown when the projectile explodes")]
    private new Light light;
    [SerializeField]
    [Tooltip("Color gradient used for the explosion animation")]
    private Gradient gradient;
    [SerializeField]
    [Tooltip("Curve used to animate the explosion")]
    private AnimationCurve radiusAnimation;
    [SerializeField]
    [Tooltip("Range used for the radius of the light")]
    private FloatRange radiusRange = new FloatRange(0.1f, 5f);
    [SerializeField]
    [Tooltip("Duration of the explosion animation")]
    private float duration = 1f;
    [SerializeField]
    [Tooltip("If true, the effect plays as soon as it is created")]
    private bool playOnAwake = true;
    [SerializeField]
    [Tooltip("If true, the effect destroys itself when it finishes playing")]
    private bool destroyOnFinish = true;

    [Space]

    [SerializeField]
    [Tooltip("Interpolator used for the explosion animations")]
    [Range(0f, 1f)]
    private float interpolator;
    #endregion

    #region Private Fields
    private float timeOfEffectStart;
    #endregion

    #region Public Methods
    public void Play()
    {
        timeOfEffectStart = Time.time;
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        if (playOnAwake)
        {
            Play();
        }
    }
    private void Update()
    {
        // Check if we are still playing the effect
        if (TimeSinceEffectStart <= duration)
        {
            interpolator = TimeSinceEffectStart / duration;
            Animate();
        }
        // If the time has exceeded the duration and we should
        // destroy on finish, then destroy the object
        else if (destroyOnFinish) Destroy(gameObject);
    }
    private void OnValidate()
    {
        Animate();
    }
    #endregion

    #region Private Methods
    private void Animate()
    {
        // Evaluate the animation curve
        float radius = radiusAnimation.Evaluate(interpolator);

        // Get the radius by lerping min and max by the animation value
        radius = Mathf.Lerp(radiusRange.min, radiusRange.max, radius);
        
        // Set the range and color of the light
        light.range = radius;
        light.color = gradient.Evaluate(interpolator);
    }
    #endregion
}
