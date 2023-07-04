using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStroke : MonoBehaviour
{
    [SerializeField]
    private float _forceMultiplier;
    [SerializeField]
    private float _cooldownMaxDuration;

    [SerializeField] 
    private float _rotationMaxSpeed;
    
    [SerializeField] 
    private bool _killVelocityBeforeNewStroke;

    [SerializeField] 
    private float _stunDuration;
    
    public UnityAction OnStroke;
    
    public UnityAction OnCooldownStart;
    public UnityAction OnCooldownEnd;

    public UnityAction OnStrokeEnabled;
    public UnityAction OnStrokeDisabled;
    
    public UnityEvent OnGroundBoost;
    public UnityEvent OnMidAirBoost;
    
    private Rigidbody _rb;
    
    [SerializeField]
    private bool _strokeEnabled;

    private bool _cooldownActive;
    private float _currentCooldownTime;
    private float _cooldownDuration;
    private float _cooldownMultiplier = 1;

    private bool _isStunned;
    private float _currentStunDuration;

    private Quaternion _baseRotation;

    void Start()
    {
        _strokeEnabled = false;
        _rb = GetComponent<Rigidbody>();

        GameplayManager.Instance.OnRoundStartCountdown += DisableStroke;
        GameplayManager.Instance.OnRoundStart += EnableStroke;
        GameplayManager.Instance.OnRoundOver += DisableStroke;
    }
    
    void Update()
    {
        if (_cooldownActive)
        {
            if (_currentCooldownTime > 0)
            {
                _currentCooldownTime -= Time.deltaTime;
            }

            else
            {
                OnCooldownEnd?.Invoke();
                _cooldownActive = false;
                EnableStroke();
            }
        }

        if (_isStunned)
        {
            if (_currentStunDuration > 0)
            {
                _currentStunDuration -= Time.deltaTime;
            }

            else
            {
                _isStunned = false;
            }
        }
    }

    private void EnableStroke()
    {
        _strokeEnabled = true;
        OnStrokeEnabled?.Invoke();
    }

    private void DisableStroke()
    {
        _strokeEnabled = false;
        OnStrokeDisabled?.Invoke();
    }
    
    private void CheckPlayerOnGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, .55f))
        {
            OnGroundBoost?.Invoke();
        }
        else
        {
            OnMidAirBoost?.Invoke();
        }
    }

    public void RotatePlayer(Vector3 _newRotation)
    {
        var desiredRotation = Quaternion.LookRotation(_newRotation, transform.up);
        var actualRotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, _rotationMaxSpeed * Time.deltaTime); ;
        _rb.MoveRotation(actualRotation);
    }
    
    public void Stroke(float _rawForce)
    {
        if (_isStunned || _strokeEnabled == false || _rawForce == 0) return;
        
        var clampRawForce = Mathf.Clamp01(_rawForce);
        StartCooldown(_cooldownMaxDuration * clampRawForce);
        CheckPlayerOnGround();
        
        if (_killVelocityBeforeNewStroke)
        {
            _rb.velocity = Vector3.zero;
        }
        
        OnStroke?.Invoke();
        _rb.AddForce(clampRawForce * _forceMultiplier * 10 * transform.forward, ForceMode.Impulse);
    }

    public void StartCooldown(float _cooldown)
    {
        _cooldownActive = true;
        var cooldown = _cooldown * _cooldownMultiplier;
        _cooldownDuration = cooldown;
        _currentCooldownTime = cooldown;
        OnCooldownStart?.Invoke();
        DisableStroke();
    }

    public void Stun()
    {
        _isStunned = true;
        _currentStunDuration = _stunDuration;
    }

    public void ChangeCooldownMultiplierForDuration(float _newMultiplier, float _duration)
    {
        _cooldownMultiplier = _newMultiplier;
        StartCoroutine(SetCooldownMultiplierToDefaultAfterDelay(_duration));
    }

    private IEnumerator SetCooldownMultiplierToDefaultAfterDelay(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        _cooldownMultiplier = 1;
    }

    public float CooldownMaxDuration => _cooldownMaxDuration;

    public float CurrentCooldownTime => _currentCooldownTime;

    public float CooldownDuration => _cooldownDuration;

    public bool StrokeEnabled => _strokeEnabled;
}
