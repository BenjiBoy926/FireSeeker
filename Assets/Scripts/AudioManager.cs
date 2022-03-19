using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    #region Private Fields
    [SerializeField]
    [Tooltip("Audio source that plays the music")]
    private AudioSource musicSource;
    [SerializeField]
    [Tooltip("Audio clip to play for the music at the start of the game")]
    private AudioClip startingMusicClip;
    [SerializeField]
    [Tooltip("Time it takes for the music to fade in")]
    private float musicFadeTime = 1f;
    [SerializeField]
    [Tooltip("Time between the each torch audio starting up")]
    private float torchStaggerTime = 0.1f;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        StaggerTorchAudio();

        // Play music from the other audio manager
        musicSource = AudioLibrary.AudioManager.PlayMusic(startingMusicClip, true);

        // Fade the source in
        musicSource.volume = 0f;
        musicSource.DOFade(1f, musicFadeTime);
    }
    #endregion

    #region Private Methods
    private void StaggerTorchAudio()
    {
        // Find all of the torches in the scene
        Torch[] torches = FindObjectsOfType<Torch>();

        // Order torches by their distance from the audio manager
        IEnumerable<Torch> orderedTorches = torches.OrderBy(torch => GetTorchDistance(torch));
        float delay = 0f;

        // Start up the audio for each torch
        foreach (Torch torch in orderedTorches)
        {
            torch.StartupAudio(delay);
            delay += torchStaggerTime;
        }
    }
    private float GetTorchDistance(Torch t)
    {
        return Vector3.Distance(t.transform.position, transform.position);
    }
    #endregion
}
