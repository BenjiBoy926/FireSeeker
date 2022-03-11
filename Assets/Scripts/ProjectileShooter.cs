using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used by the protagonist to launch fireballs that 
/// defeat enemies and light torches
/// </summary>
public class ProjectileShooter : MonoBehaviour
{
    #region Private Properties
    private Vector2 ReticleScreenPoint => mainCamera.WorldToScreenPoint(reticle.position);
    private float RaycastDistance => mainCamera.farClipPlane - mainCamera.nearClipPlane;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the camera used by the fire heart " +
        "to launch fireballs")]
    private Camera mainCamera;
    [SerializeField]
    [Tooltip("Physics layer to hit with the camera ray " +
        "that aims the projectiles")]
    private LayerMask aimRayLayer;
    [SerializeField]
    [Tooltip("Reticle to fire projectiles from")]
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
            // Fire a ray out of the screen from the reticle
            Ray screenRay = mainCamera.ScreenPointToRay(ReticleScreenPoint);
            bool hit = Physics.Raycast(
                screenRay, 
                out RaycastHit hitInfo,
                RaycastDistance, 
                aimRayLayer);
            Vector3 aimPoint;

            // If the ray hit something then aim at the point of the hit
            if (hit) aimPoint = hitInfo.point;
            // If the ray hit nothing then aim at the farthest reach of the ray
            else aimPoint = screenRay.GetPoint(RaycastDistance);

            Debug.DrawLine(aimPoint, screenRay.origin, Color.green, 0.5f);
            Debug.DrawLine(aimPoint, transform.position, Color.red, 0.5f);
        }
    }
    #endregion
}
