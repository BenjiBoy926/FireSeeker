using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [SerializeField]
    [Tooltip("Audio source to play from when a projectile is launched")]
    private AudioSource audioSource;

    [field: Space]

    [field: SerializeField]
    [field: Tooltip("Event invoked when a projectile shot from this shooter " +
        "hits a projectile receiver")]
    public UnityEvent<ProjectileReceiver> ProjectileReceivedEvent { get; private set; }
    #endregion

    #region Private Fields
    private LightTransition[] transitions;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        transitions = GetComponentsInChildren<LightTransition>(true);
    }
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
            projectile.owner = this;

            // Play all light transitions
            PlayTransitions();

            // Play the audio source
            audioSource.Play();
        }
    }
    #endregion

    #region Private Methods
    private void PlayTransitions()
    {
        foreach (LightTransition transition in transitions)
        {
            transition.Play();
        }
    }
    #endregion
}
