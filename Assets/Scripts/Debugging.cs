using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Component halo = GetComponent("Halo");
        Debug.Log(halo, halo);
        Type haloType = halo.GetType();
        Debug.Log($"Halo type: \n\t{haloType.Name}\n\t{haloType.FullName}");
        MemberInfo[] members = haloType.GetMembers();

        string message = "Halo public members";

        foreach (MemberInfo member in members)
        {
            message += $"\n{member.Name}";
            message += $"\n\tType: {member.MemberType}";
        }

        Debug.Log(message);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
