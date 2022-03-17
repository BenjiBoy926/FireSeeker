using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to mark a point that the player can respawn to
/// </summary>
public class RespawnPoint : MonoBehaviour
{
    #region Public Properties
    [field: SerializeField]
    [field: Tooltip("Torch that lights up when this respawn point is set")]
    public Torch Torch { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Order of the respawn point in the respawn system")]
    public int Order { get; private set; }
    #endregion

    #region Monobehaviour Messages
    private void OnTriggerEnter(Collider other)
    {
        // Try to get a respawner in the parent of the object
        Respawner hit = other.GetComponentInParent<Respawner>();

        // If we get a respawner, set its point to this
        if (hit)
        {
            hit.SetPoint(this);

            // Light up the torch now that this point is set
            Torch.LightUp();
        }
    }
    #endregion
}
