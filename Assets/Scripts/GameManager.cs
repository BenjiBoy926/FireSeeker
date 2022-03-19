using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Public Properties
    [field: SerializeField]
    [field: Tooltip("Reference to the first respawn point that the player should look at")]
    public RespawnPoint FirstRespawnPoint { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Torch of the final respawn point that the player should hit")]
    public RespawnPoint FinalRespawnPoint { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Skybox to switch out when the player gets all respawn points lit up")]
    public Material Skybox { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Directional light representing the sun")]
    public Light Sun;
    [field: SerializeField]
    [field: Tooltip("Time it takes to fade in and out at the end of the game")]
    public float FadeTime { get; private set; } = 2f;
    #endregion

    #region Private Fields
    private PlayerManager playerManager;
    private bool firstTimeLanding = true;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        FinalRespawnPoint.Torch.LightUpEvent.AddListener(OnFinalRespawnPointHit);
        Sun.enabled = false;
        playerManager = FindObjectOfType<PlayerManager>();

        // Look at a point above the first respawn point
        playerManager.CameraController.LookAt(
            new Vector3(
                FirstRespawnPoint.transform.position.x, 
                playerManager.transform.position.y + 5f, 
                FirstRespawnPoint.transform.position.z));
        playerManager.CameraController.controlsEnabled = false;

        // Listen for player's first landing
        playerManager.Character.landedEvent.AddListener(OnPlayerLanded);
    }
    #endregion

    #region Private Methods
    private void OnPlayerLanded()
    {
        if (firstTimeLanding)
        {
            playerManager.CameraController.controlsEnabled = true;
            firstTimeLanding = false;
        }
    }
    private void OnFinalRespawnPointHit()
    {
        StartCoroutine(BrightEnvironmentChange());
    }
    private IEnumerator BrightEnvironmentChange()
    {
        // Wait until the torch transition is finished
        yield return new WaitForSeconds(FinalRespawnPoint.Torch.Transition.Duration);

        // Have the audio manager switch tracks here

        // Wait for the panel to fade in
        yield return FadingPanel.FadeIn(FadeTime).WaitForCompletion();

        // Transform the area into a pretty sunrise
        RenderSettings.skybox = Skybox;
        RenderSettings.ambientMode = AmbientMode.Skybox;
        Sun.enabled = true;
        playerManager.ReverbFilter.enabled = false;

        // Fade out the panel
        FadingPanel.FadeOut(FadeTime);
    }
    #endregion
}
