using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used by the protagonist to launch fireballs that 
/// defeat enemies and light torches
/// </summary>
public class FireHeart : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the camera used by the fire heart " +
        "to launch fireballs")]
    private Camera mainCamera;
    [SerializeField]
    [Tooltip("Reference to the transform of the UI object " +
        "that the fire casts a ray from")]
    private Transform reticle;
    [SerializeField]
    [Tooltip("Velocity of the fireball " +
        "when launched from the heart")]
    private float launchVelocity = 20f;
    #endregion

    #region Monobehaviour Messages
    private void Update()
    {
        // Check if the "Fire1" button is pressed
        if (Input.GetButtonDown("Fire1"))
        {

        }
    }
    #endregion
}
