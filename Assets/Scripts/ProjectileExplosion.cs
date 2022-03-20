using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    #region Private Editor Fields
    [field: SerializeField]
    [field: Tooltip("Transition used to create a fun effect for the explosion")]
    public LightTransition Transition { get; private set; }
    [SerializeField]
    [Tooltip("Physics layers that the explosion can hit")]
    private LayerMask explosionMask;
    [SerializeField]
    [Tooltip("Range of the explosion")]
    private float explosionRadius = 5f;
    #endregion

    #region Public Methods
    public void Explode(ProjectileShooter owner, Projectile projectile)
    {
        // Overlap all colliders in the range
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionMask, QueryTriggerInteraction.Collide);

        foreach (Collider collider in colliders)
        {
            ProjectileReceiver receiver = collider.GetComponent<ProjectileReceiver>();

            // Notify all receivers that a projectile hit them
            if (receiver)
            {
                receiver.ProjectileHitEvent.Invoke(projectile);
                owner.ProjectileReceivedEvent.Invoke(receiver);
            }
        }
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Destroy self when the transition is finished
        Transition.TransitionCompleteEvent.AddListener(DestroySelf);
    }
    #endregion

    #region Private Methods
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
    #endregion
}
