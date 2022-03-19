using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    #region Private Fields
    [field: SerializeField]
    [field: Tooltip("Audio source that plays the music")]
    public AudioSource MusicSource { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Audio clip to play at the end of the game")]
    public AudioClip EndingMusicClip { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Time it takes for the music to fade in")]
    public float MusicFadeTime { get; private set; } = 1f;
    [field: SerializeField]
    [field: Tooltip("Time between the each torch audio starting up")]
    public float TorchStaggerTime { get; private set; } = 0.1f;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        StaggerTorchAudio();

        // Fade the source in
        MusicSource.volume = 0f;
        MusicSource.DOFade(1f, MusicFadeTime);
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
            delay += TorchStaggerTime;
        }
    }
    private float GetTorchDistance(Torch t)
    {
        return Vector3.Distance(t.transform.position, transform.position);
    }
    #endregion
}
