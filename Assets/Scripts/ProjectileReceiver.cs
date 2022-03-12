using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileReceiver : MonoBehaviour
{
    #region Public Properties
    [field: SerializeField]
    [field: Tooltip("Event invoked when a projectile hits this receiver")]
    public UnityEvent ProjectileHitEvent { get; private set; }
    #endregion
}
