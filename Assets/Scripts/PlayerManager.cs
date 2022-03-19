using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerManager : MonoBehaviour
{
    #region Private Editor Fields
    [field: SerializeField]
    [field: Tooltip("Script used to control the third person character")]
    public ThirdPersonCharacter Character { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Script that enables user control for the character")]
    public ThirdPersonUserControl UserControl { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Script that enables camera controls for the user")]
    public CameraController CameraController { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Script that detects collisions for the player")]
    public CollisionEvents BodyCollisions { get; private set; }
    [field: SerializeField]
    [field: Tooltip("Reference to the filter that creates reverb")]
    public AudioReverbFilter ReverbFilter { get; private set; }
    #endregion
}
