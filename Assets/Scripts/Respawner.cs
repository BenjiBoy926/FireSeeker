using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using DG.Tweening;

public class Respawner : MonoBehaviour
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
    private Rigidbody decoy;
    [SerializeField]
    [Tooltip("Speed of the decoy when it spins")]
    private float decoySpinSpeed = 30f;
    [SerializeField]
    [Tooltip("If the player falls below this level, then a respawn is triggered")]
    private float killDepth = -25;
    [SerializeField]
    [Tooltip("Time to wait after revealing the decoy to send the body to the respawn point")]
    private float decoyRevealTime = 1f;
    [SerializeField]
    [Tooltip("Time after respawn finishes that the decoy stays in place before disabling")]
    private float decoyConcealTime = 0.8f;
    [SerializeField]
    [Tooltip("Time it takes for the body to move to the respawn point")]
    private float moveTime = 2f;
    [SerializeField]
    [Tooltip("Ease of the animation that moves the body to the respawn point")]
    private Ease animationEase;
    #endregion

    #region Private Fields
    private Coroutine respawnRoutine;
    private TrailRenderer[] decoyTrails;
    #endregion

    #region Public Methods
    public void SetPoint(RespawnPoint respawnPoint)
    {
        // Only set the current point if we don't have a current point yet
        // or the point comes after the current point
        if (currentPoint == null || currentPoint.Order < respawnPoint.Order)
        {
            currentPoint = respawnPoint;
        }
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        decoy.gameObject.SetActive(false);
        decoyTrails = decoy.GetComponentsInChildren<TrailRenderer>(true);
    }
    private void Update()
    {
        if (respawnRoutine == null && (body.position.y < killDepth || Input.GetKeyDown(KeyCode.Backspace)))
        {
            HandleRespawn();
        }
    }
    #endregion

    #region Private Methods
    private void HandleRespawn()
    {
        if (currentPoint) respawnRoutine = StartCoroutine(RespawnRoutine());
        else respawnRoutine = StartCoroutine(RestartRoutine());
    }
    private IEnumerator RespawnRoutine()
    {
        // Disable the body and enable the decoy
        body.gameObject.SetActive(false);
        decoy.gameObject.SetActive(true);

        // Reset the decoy
        SetDecoyTrailsEnabled(false);
        decoy.angularVelocity = Vector3.zero;
        decoy.position = body.position;
        decoy.transform.forward = currentPoint.transform.position - decoy.position;

        // Wait after the decoy reveal
        yield return new WaitForSeconds(decoyRevealTime);

        // Ease the decoy and body towards the respawn point
        SetDecoyTrailsEnabled(true);
        decoy.angularVelocity = decoy.transform.forward * decoySpinSpeed;
        decoy.DOMove(currentPoint.transform.position, moveTime).SetEase(animationEase);

        // Wait for the body to get to the respawn point
        yield return body
            .DOMove(currentPoint.transform.position, moveTime)
            .SetEase(animationEase)
            .WaitForCompletion();

        // Enable body again and disable decoy
        body.gameObject.SetActive(true);

        // Disable and re-enable the decoy to repeat the reveal effect
        decoy.gameObject.SetActive(false);
        decoy.gameObject.SetActive(true);

        // Wait for the second reveal
        yield return new WaitForSeconds(decoyConcealTime);

        // Finall, disable the decoy
        decoy.gameObject.SetActive(false);

        // Remove the respawn routine
        respawnRoutine = null;
    }
    private IEnumerator RestartRoutine()
    {
        // Wait for the panel to fade in
        yield return FadingPanel.FadeIn().WaitForCompletion();

        // Reload this scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void SetDecoyTrailsEnabled(bool enabled)
    {
        foreach (TrailRenderer trail in decoyTrails)
        {
            trail.enabled = enabled;
        }
    }
    #endregion
}
