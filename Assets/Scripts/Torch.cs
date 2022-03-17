using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Torch : MonoBehaviour
{
    #region Public Properties
    [field: SerializeField]
    [field: Tooltip("Transition between the torch becoming lit and unlit")]
    [field: FormerlySerializedAs("transition")]
    public LightTransition Transition { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Determines if the torch is currently lit or unlit")]
    [field: FormerlySerializedAs("list")]
    public bool Lit { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Event invoked when the torch is lit up")]
    public UnityEvent LightUpEvent { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Event invoked when the torch is snuffed out")]
    public UnityEvent SnuffOutEvent { get; private set; }
    #endregion

    #region Public Methods
    public void LightUp()
    {
        if (!Lit)
        {
            Lit = true;
            Transition.Play();
            LightUpEvent.Invoke();
        }
    }
    public void SnuffOut()
    {
        if (Lit)
        {
            Lit = false;
            Transition.PlayReversed();
            SnuffOutEvent.Invoke();
        }
    }
    #endregion

    #region Monobehaviour Messages
    private void OnValidate()
    {
        if (Transition)
        {
            if (Lit) Transition.PlayReversed();
            else Transition.Play();
            Transition.OnValidate();
        }
    }
    #endregion
}
