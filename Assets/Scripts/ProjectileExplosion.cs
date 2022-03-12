using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Physics layers that the explosion can hit")]
    private LayerMask explosionMask;
    [SerializeField]
    [Tooltip("Range of the explosion")]
    private float explosionRange = 10f;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Overlap all colliders in the range
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange, explosionMask, QueryTriggerInteraction.Collide);

        foreach (Collider collider in colliders)
        {
            ProjectileReceiver receiver = collider.GetComponent<ProjectileReceiver>();

            // Notify all receivers that a projectile hit them
            if (receiver)
            {
                receiver.ProjectileHitEvent.Invoke();
            }
        }
    }
    #endregion
}
