using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TorchGroup : MonoBehaviour
{
    #region Public Properties
    [field: SerializeField]
    [field: Tooltip("List of torches in the group")]
    public Torch[] Torches { get; private set; } = new Torch[0];
    [field: SerializeField]
    [field: Tooltip("List of torches in the group")]
    public UnityEvent AllTorchesLitEvent { get; private set; }
    #endregion

    #region Private Fields
    private int countTorchesLit;
    #endregion

    #region Monobehaviour Messages
    private void Start()
    {
        // Subscribe to the torch lit and snuff out events
        foreach (Torch torch in Torches)
        {
            torch.LightUpEvent.AddListener(OnTorchLitUp);
            torch.SnuffOutEvent.AddListener(OnTorchSnuffedOut);
        }

        countTorchesLit = Torches.Count(torch => torch.Lit);
        CheckAllTorchesLit();
    }
    private void OnDrawGizmosSelected()
    {
        foreach (Torch torch in Torches)
        {
            // Set color based on if the torch is lit
            if (torch.Lit)
            {
                Gizmos.color = Color.yellow;
            }
            else Gizmos.color = Color.cyan;

            // Draw a line from the group owner to the torch
            Gizmos.DrawLine(transform.position, torch.transform.position);
        }
    }
    #endregion

    #region Private Methods
    private void OnTorchLitUp()
    {
        countTorchesLit++;
        CheckAllTorchesLit();
    }
    private void OnTorchSnuffedOut()
    {
        countTorchesLit--;
    }
    private void CheckAllTorchesLit()
    {
        if (countTorchesLit >= Torches.Length)
        {
            AllTorchesLitEvent.Invoke();
        }
    }
    #endregion
}
