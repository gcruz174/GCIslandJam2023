using System;
using UnityEngine;

public class GenerateImpulse : MonoBehaviour
{
    private void Awake()
    {
        // Call cinemachine impulse generator
        var impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        impulseSource.GenerateImpulse();
    }
}
