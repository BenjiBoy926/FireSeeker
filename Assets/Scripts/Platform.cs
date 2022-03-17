using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{
    #region Public Properties
    [field: SerializeField]
    [field: Tooltip("Amount of time it takes for the platform " +
        "to raise and lower")]
    public float MoveTime { get; private set; } = 3f;
    [field: SerializeField]
    [field: Tooltip("Vertical position of the platform when it is raised")]
    public float RaisedPosition { get; private set; } = 30f;
    [field: SerializeField]
    [field: Tooltip("Position of the platform when it is lowered")]
    public float LoweredPosition { get; private set; } = 0f;
    [field: SerializeField]
    [field: Tooltip("Whether the drawbridge is currently raised or lowered")]
    public bool Lowered { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Ease to use for the platform lowering")]
    public Ease LowerAnimationEase { get; private set; } = Ease.OutCubic;
    [field: SerializeField]
    [field: Tooltip("Ease to use for the platform rasing")]
    public Ease RaiseAnimationEase { get; private set; } = Ease.InBack;
    #endregion

    #region Monobehaviour Messages
    private void OnValidate()
    {
        if (Lowered) transform.position = new Vector3(transform.position.x, LoweredPosition, transform.position.z);
        else transform.position = new Vector3(transform.position.x, RaisedPosition, transform.position.z);
    }
    #endregion

    #region Public Methods
    public void Lower()
    {
        if (!Lowered)
        {
            transform.DOKill();
            transform.DOMoveY(LoweredPosition, MoveTime).SetEase(LowerAnimationEase);
            Lowered = true;
        }
    }
    public void Raise()
    {
        if (Lowered)
        {
            transform.DOKill();
            transform.DOMoveY(RaisedPosition, MoveTime).SetEase(RaiseAnimationEase);
            Lowered = false;
        }
    }
    #endregion
}
