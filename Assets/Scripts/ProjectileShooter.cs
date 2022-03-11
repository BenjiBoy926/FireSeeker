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
    [Tooltip("Prefab to instantiate for the fireball")]
    private Projectile projectilePrefab;
    [SerializeField]
    [Tooltip("Reticle to fire projectiles from")]
    private Transform reticle;
    [SerializeField]
    [Tooltip("Base velocity of the fireball " +
        "when launched from the heart")]
    private float baseLinearVelocity = 20;
    [SerializeField]
    [Tooltip("Spinning velocity of the fireball")]
    private float baseAngularVelocity = 3;
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

            // Create a projectile and launch it towards the target point
            Vector3 toTarget = aimPoint - transform.position;
            Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.Launch(toTarget, baseLinearVelocity, baseAngularVelocity);
        }
    }
    #endregion
}
