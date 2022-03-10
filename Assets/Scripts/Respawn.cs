using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    #region Public Fields
    public string respawnTag = "Respawn";
    public float killDepth = -25;
    #endregion

    #region Monobehaviour Messages
    private void Update()
    {
        if (transform.position.y < killDepth)
        {
            GameObject respawn = GameObject.FindWithTag(respawnTag);

            // If a respawn point was found then set this position
            // to the respawn's position
            if (respawn)
            {
                transform.position = respawn.transform.position;
            }
            else Debug.LogWarning(
                $"Cannot respawn because no object with the tag " +
                $"'{respawnTag}' could be found in the scene. Make " +
                $"sure a respawn point with the tag '{respawnTag}' " +
                $"exists in the scene, or change the respawn tag");
        }
    }
    #endregion
}
