using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Script used to pick up hits from projectiles that can light and unlight the torch")]
    private ProjectileReceiver projectileReceiver;
    [SerializeField]
    [Tooltip("Transition between the torch becoming lit and unlit")]
    private LightTransition transition;
    [SerializeField]
    [Tooltip("Determines if the torch is currently lit or unlit")]
    private bool lit;
    #endregion

    #region Public Methods
    public void LightUp()
    {
        lit = true;
        transition.Play();
    }
    public void SnuffOut()
    {
        lit = false;
        transition.PlayReversed();
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        projectileReceiver.ProjectileHitEvent.AddListener(LightUp);
    }
    private void OnValidate()
    {
        if (transition)
        {
            if (lit) transition.PlayReversed();
            else transition.Play();
            transition.OnValidate();
        }
    }
    #endregion
}
