using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Drawbridge : MonoBehaviour
{
    #region Public Properties
    [field: SerializeField]
    [field: Tooltip("Amount of time it takes for the bridge " +
        "to raise and lower")]
    public float MoveTime { get; private set; } = 3f;
    [field: SerializeField]
    [field: Tooltip("Direction of the drawbridge when it is " +
        "raised, meaning it cannot be crossed")]
    public Vector3 RaisedOrientation { get; private set; } = Vector3.up;
    [field: SerializeField]
    [field: Tooltip("Direction of the drawbridge when it is " +
        "lowered, meaning it can be crossed")]
    public Vector3 LoweredOrientation { get; private set; } = Vector3.forward;
    [field: SerializeField]
    [field: Tooltip("Whether the drawbridge is currently raised or lowered")]
    public bool Lowered { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Ease to use for the drawbridge lowering")]
    public Ease LowerAnimationEase { get; private set; } = Ease.OutBounce;
    [field: SerializeField]
    [field: Tooltip("Ease to use for the drawbridge rasing")]
    public Ease RaiseAnimationEase { get; private set; } = Ease.InCubic;
    [field: SerializeField]
    [field: Tooltip("Audio clip that plays when the drawbridge is lowered")]
    public AudioClip PositionChangeClip { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Audio source that plays the audio for the bridge")]
    public AudioSource AudioSource { get; private set; }
    #endregion

    #region Monobehaviour Messages
    private void OnValidate()
    {
        if (Lowered) transform.eulerAngles = LoweredOrientation;
        else transform.eulerAngles = RaisedOrientation;
    }
    #endregion

    #region Public Methods
    public void Lower()
    {
        if (!Lowered)
        {
            transform.DOKill();
            transform.DORotate(LoweredOrientation, MoveTime).SetEase(LowerAnimationEase);
            Lowered = true;

            // Play the position change sound
            AudioSource.clip = PositionChangeClip;
            AudioSource.Play();
        }
    }
    public void Raise()
    {
        if (Lowered)
        {
            transform.DOKill();
            transform.DORotate(RaisedOrientation, MoveTime).SetEase(RaiseAnimationEase);
            Lowered = false;

            // Play the position change sound
            AudioSource.clip = PositionChangeClip;
            AudioSource.Play();
        }
    }
    #endregion
}
