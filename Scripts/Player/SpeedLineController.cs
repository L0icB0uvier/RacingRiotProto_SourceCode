using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SpeedLineController : MonoBehaviour
{
    [SerializeField] 
    private float _minVelocity;

    [SerializeField] private float _speedFactor;
    
    private Rigidbody _rb;
    private ParticleSystem _particleSystem;
    private ParticleSystem.EmissionModule _emissionModule;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _emissionModule = _particleSystem.emission;
    }

    void Start()
    {
        PlayerManager.Instance.OnPlayersSpawned += GetPlayerRigidbody;
        GameplayManager.Instance.OnRoundStart += Activate;
        GameplayManager.Instance.OnRoundOver += Deactivate;
        Deactivate();
    }

    private void Activate()
    {
        enabled = true;
        _particleSystem.Play();
    }

    private void Deactivate()
    {
        enabled = false;
        _particleSystem.Stop();
    }

    private void GetPlayerRigidbody()
    {
        _rb = PlayerManager.Instance.OwnerPlayer.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (_rb == null || _rb.velocity.sqrMagnitude < _minVelocity * _minVelocity)
        {
            _emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(0);
            return;
        }
        
        float emissionRate = _rb.velocity.sqrMagnitude * _speedFactor;
        _emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(emissionRate);
    }
}
