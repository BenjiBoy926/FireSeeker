using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Public Properties
    public RespawnPoint FirstRespawnPoint => respawnPoints.OrderBy(point => point.Order).First();
    public RespawnPoint LastRespawnPoint => respawnPoints.OrderBy(point => point.Order).Last();
    #endregion

    #region Editor Properties
    [field: SerializeField]
    [field: Tooltip("Reference to the script that managers audio")]
    public AudioManager Audio { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Canvas with the final goodbye message for the player")]
    public GameObject GoodbyeCanvas { get; private set; }
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
    private RespawnPoint[] respawnPoints;
    private bool firstTimeLanding = true;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Find player and respawn points
        playerManager = FindObjectOfType<PlayerManager>();
        respawnPoints = FindObjectsOfType<RespawnPoint>();

        // Listen for last respawn point lit up
        GoodbyeCanvas.SetActive(false);
        LastRespawnPoint.Torch.LightUpEvent.AddListener(OnFinalRespawnPointHit);
        Sun.enabled = false;

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
        yield return new WaitForSeconds(LastRespawnPoint.Torch.Transition.Duration);

        // Have the audio manager switch tracks here
        Audio.MusicSource.DOFade(0f, FadeTime);

        // Wait for the panel to fade in
        yield return FadingPanel.FadeIn(FadeTime).WaitForCompletion();

        // Transform the area into a pretty sunrise
        RenderSettings.skybox = Skybox;
        RenderSettings.ambientMode = AmbientMode.Skybox;
        Sun.enabled = true;
        playerManager.ReverbFilter.enabled = false;
        GoodbyeCanvas.SetActive(true);

        // Fade in the ending music
        Audio.MusicSource.clip = Audio.EndingMusicClip;
        Audio.MusicSource.Play();
        Audio.MusicSource.DOFade(1f, FadeTime);

        // Fade out the panel
        FadingPanel.FadeOut(FadeTime);
    }
    #endregion
}
