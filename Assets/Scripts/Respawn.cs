using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Respawn : MonoBehaviour
{
    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the current respawn point for the player")]
    private RespawnPoint currentPoint;
    [SerializeField]
    [Tooltip("Transform of the object to check and respawn when it falls too low")]
    private Transform body;
    [SerializeField]
    [Tooltip("Object that activates to create a respawn effect")]
    private Transform decoy;
    [SerializeField]
    [Tooltip("If the player falls below this level, then a respawn is triggered")]
    private float killDepth = -25;
    [SerializeField]
    [Tooltip("Time to wait after revealing the decoy to send the body to the respawn point")]
    private float decoyRevealTime = 1f;
    [SerializeField]
    [Tooltip("Time it takes for the body to move to the respawn point")]
    private float moveTime = 2f;
    [SerializeField]
    [Tooltip("Ease of the animation that moves the body to the respawn point")]
    private Ease animationEase;
    #endregion

    #region Private Fields
    private Coroutine respawnRoutine;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        decoy.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (body.position.y < killDepth)
        {
            // If there is a current point and no respawn routine,
            // then start the respawn
            if (currentPoint)
            {
                if (respawnRoutine == null)
                {
                    respawnRoutine = StartCoroutine(RespawnRoutine());
                }
            }
            // If there is not respawn point then we restart the scene
            else
            {

            }
        }
    }
    #endregion

    #region Private Methods
    private IEnumerator RespawnRoutine()
    {
        // Disable the body and enable the decoy
        body.gameObject.SetActive(false);
        decoy.gameObject.SetActive(true);
        decoy.position = body.position;

        // Wait after the decoy reveal
        yield return new WaitForSeconds(decoyRevealTime);

        // Ease the decoy and body towards the respawn point
        decoy.DOMove(currentPoint.transform.position, moveTime).SetEase(animationEase);

        // Wait for the body to get to the respawn point
        yield return body
            .DOMove(currentPoint.transform.position, moveTime)
            .SetEase(animationEase)
            .WaitForCompletion();

        // Enable body again and disable decoy
        body.gameObject.SetActive(true);
        decoy.gameObject.SetActive(false);

        // Remove the respawn routine
        respawnRoutine = null;
    }
    #endregion
}
