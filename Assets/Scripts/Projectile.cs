using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Private Properties
    private float Age => Time.time - timeOfCreation;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Rigidbody of the projectile")]
    private Rigidbody body;
    [SerializeField]
    [Tooltip("Projectile will destroy itself after this amount of time")]
    private float maxLifetime = 3f;
    [SerializeField]
    [Tooltip("Multiplier for the base linear velocity")]
    private float linearVelocityMultiplier = 1f;
    [SerializeField]
    [Tooltip("Multiplied to the base angular velocity to get the angular velocity")]
    private float angularVelocityMultiplier = 1f;
    #endregion

    #region Private Fields
    private float timeOfCreation;
    #endregion

    #region Public Methods
    public void Launch(Vector3 direction, float baseLinearVelocity, float baseAngularVelocity)
    {
        // Normalize direction
        direction = direction.normalized;

        // Point towards direction
        body.transform.forward = direction;

        // Set velocity and angulary velocity to go towards direction
        body.velocity = baseLinearVelocity * linearVelocityMultiplier * direction.normalized;
        body.angularVelocity = baseAngularVelocity * angularVelocityMultiplier * direction.normalized;
    }
    public void Explode()
    {
        // Create an explosion at this point

        // Destroy the projectile
        Destroy(gameObject);
    }
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        timeOfCreation = Time.time;
    }
    private void Update()
    {
        if (Age >= maxLifetime)
        {
            Explode();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }
    #endregion
}
