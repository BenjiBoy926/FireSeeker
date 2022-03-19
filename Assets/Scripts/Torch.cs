using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using DG.Tweening;

public class Torch : MonoBehaviour
{
    #region Public Properties
    [field: SerializeField]
    [field: Tooltip("Transition between the torch becoming lit and unlit")]
    [field: FormerlySerializedAs("transition")]
    public LightTransition Transition { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Determines if the torch is currently lit or unlit")]
    [field: FormerlySerializedAs("list")]
    public bool Lit { get; private set; }
    [field: SerializeField]
    [field: Tooltip("List of audio sources that play while the torch is lit up")]
    public AudioSource[] LitUpAudio { get; private set; }
    [field: SerializeField]
    [field: Tooltip("List of audio sources that play while the torch is snuffed out")]
    public AudioSource[] SnuffedOutAudio { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Event invoked when the torch is lit up")]
    public UnityEvent LightUpEvent { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Event invoked when the torch is snuffed out")]
    public UnityEvent SnuffOutEvent { get; private set; }
    #endregion

    #region Public Methods
    public void StartupAudio(float delay)
    {
        foreach (AudioSource audio in LitUpAudio)
        {
            audio.PlayDelayed(delay);
        }
        foreach (AudioSource audio in SnuffedOutAudio)
        {
            audio.PlayDelayed(delay);
        }
    }
    public void LightUp()
    {
        if (!Lit)
        {
            // Play the light transition
            Lit = true;
            Transition.Play();

            // Fade from snuffed out to lit up audio
            CrossFadeAudio(SnuffedOutAudio, LitUpAudio);

            // Invoke the event
            LightUpEvent.Invoke();
        }
    }
    public void SnuffOut()
    {
        if (Lit)
        {
            // Play the light transition
            Lit = false;
            Transition.PlayReversed();

            // Fade from lit up audio to snuffed out audio
            CrossFadeAudio(LitUpAudio, SnuffedOutAudio);

            // Invoke the event
            SnuffOutEvent.Invoke();
        }
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        foreach (AudioSource audio in LitUpAudio)
        {            
            if (Lit) audio.volume = 1;
            else audio.volume = 0;
        }
        foreach (AudioSource audio in SnuffedOutAudio)
        {
            if (Lit) audio.volume = 0;
            else audio.volume = 1;
        }
    }
    private void OnValidate()
    {
        if (Transition)
        {
            if (Lit) Transition.PlayReversed();
            else Transition.Play();
            Transition.OnValidate();
        }
    }
    #endregion

    #region Private Editor Fields
    private void CrossFadeAudio(AudioSource[] from, AudioSource[] to)
    {
        foreach (AudioSource audio in from)
        {
            audio.DOKill();
            audio.DOFade(0f, Transition.Duration);
        }
        foreach (AudioSource audio in to)
        {
            audio.DOKill();
            audio.DOFade(1f, Transition.Duration);
        }
    }
    #endregion
}
