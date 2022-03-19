using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour
{
    #region Public Typedefs
    [System.Serializable]
    public class FootConfig
    {
        public Transform transform;
        public AudioSource audioSource;
        private bool previouslyOnGround = false;
        private bool currentlyOnGround = false;

        public void Update(float rayDistance, LayerMask mask, AudioClip[] sounds)
        {
            // Shoot a raycast down from the foot to determine if we are on the ground
            Ray ray = new Ray(transform.position, -transform.up);
            currentlyOnGround = Physics.Raycast(ray, rayDistance, mask);

#if UNITY_EDITOR
            Debug.DrawRay(ray.origin, ray.direction * rayDistance);
#endif

            // If we were not previously on the ground but we are now then play
            // a random footfall sound
            if (!previouslyOnGround && currentlyOnGround)
            {
                audioSource.clip = sounds[Random.Range(0, sounds.Length)];
                audioSource.Play();
            }

            // Set previously on ground state before moving on
            previouslyOnGround = currentlyOnGround;
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Distance of the ray to cast from the feet to detect a footfall")]
    private float footRayDistance = 0.1f;
    [SerializeField]
    [Tooltip("Layer mask used to filter the colliders that a foot detects")]
    private LayerMask footRayMask;
    [SerializeField]
    [Tooltip("Configuration for the left foot")]
    private FootConfig leftFoot;
    [SerializeField]
    [Tooltip("Configuration for the right foot")]
    private FootConfig rightFoot;
    [SerializeField]
    [Tooltip("List of footstep sounds to use for the character")]
    private AudioClip[] footstepSounds;

    [Space]

    [SerializeField]
    [Tooltip("Audio source used to play jumping sounds")]
    private AudioSource jumpSource;
    [SerializeField]
    [Tooltip("List of jump sounds to use for the character")]
    private AudioClip[] jumpSounds;
    [SerializeField]
    [Tooltip("List of sounds to use for the character landing on the ground")]
    private AudioClip[] landingSounds;
    #endregion

    #region Public Methods
    public void PlayJumpSound()
    {
        jumpSource.clip = jumpSounds[Random.Range(0, jumpSounds.Length)];
        jumpSource.Play();
    }
    public void PlayLandingSound()
    {
        jumpSource.clip = landingSounds[Random.Range(0, landingSounds.Length)];
        jumpSource.Play();
    }
    #endregion

    #region Monobehaviour Messages
    private void Update()
    {
        leftFoot.Update(footRayDistance, footRayMask, footstepSounds);
        rightFoot.Update(footRayDistance, footRayMask, footstepSounds);
    }
    #endregion
}
